using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.WebGL;
using Bomberman.Client.Networking;
using Bomberman.Shared;

namespace Bomberman.Client
{
    public class GameRenderer
    {
        public const int PIXELS_PER_UNIT = 64;
        public const float SPRITE_SIZE_IN_SHEET = 0.125f;
        public const int MAX_GAME_OBJECTS = 200;

        private BECanvasComponent _canvasReference;

        WebGLContext _context;

        public long canvasWidth = 0;
        public long canvasHeight = 0;

        byte[] byteArr;

        byte[] textureData = new byte[4] { 0, 0, 255, 0 };

        WebGLUniformLocation u_matrix_location;

        private const string VS_SOURCE =
            "attribute vec3 aPos;" +
            "attribute vec2 aTex;" +
            "varying vec2 vTex;" +
            "uniform mat4 u_matrix;" +

            "void main() {" +
                "gl_Position = u_matrix * vec4(aPos, 1.0);" +
                "vTex = aTex;" +
            "}";

        private const string FS_SOURCE = "precision mediump float;" +
                                         "varying vec2 vTex;" +
                                         "uniform sampler2D u_texture;" +

                                         "void main() {" +
                                            "vec4 texColor = texture2D(u_texture, vTex);" +
                                            "if(texColor.a< 0.95)" +
                                                "discard;" + 
                                            "gl_FragColor = texture2D(u_texture, vTex);" +
                                         "}";

        Vector3 transVector = new Vector3((float)-1, (float)-1, 0);

        WebGLBuffer vertexBuffer;

        private int gameObjectCount = 0;
        private float[] vertices = new float[6 * 6 * MAX_GAME_OBJECTS];


        public GameRenderer(BECanvasComponent CanvasReference)
        {
            _canvasReference = CanvasReference;

            byteArr = new byte[vertices.Length * System.Runtime.InteropServices.Marshal.SizeOf<float>()];
        }


        public async Task Initialize()
        {
            _context = await _canvasReference.CreateWebGLAsync(new WebGLContextAttributes
            {
                PowerPreference = WebGLContextAttributes.POWER_PREFERENCE_HIGH_PERFORMANCE,
                Antialias = false
            });

            vertexBuffer = await _context.CreateBufferAsync();
            await _context.BindBufferAsync(BufferType.ARRAY_BUFFER, vertexBuffer);

            var program = await this.InitProgramAsync(this._context, VS_SOURCE, FS_SOURCE);

            var positionLocation = await _context.GetAttribLocationAsync(program, "aPos");
            var texcoordLocation = await _context.GetAttribLocationAsync(program, "aTex");

            await _context.VertexAttribPointerAsync((uint)positionLocation, 3, DataType.FLOAT, false, 6 * sizeof(float), 0);
            await _context.VertexAttribPointerAsync((uint)texcoordLocation, 2, DataType.FLOAT, false, 6 * sizeof(float), 3 * sizeof(float));
            await _context.EnableVertexAttribArrayAsync((uint)positionLocation);
            await _context.EnableVertexAttribArrayAsync((uint)texcoordLocation);

            await _context.UseProgramAsync(program);

            var texture = await _context.CreateTextureAsync();
            await _context.BindTextureAsync(TextureType.TEXTURE_2D, texture);

        }

        private async Task<WebGLProgram> InitProgramAsync(WebGLContext gl, string vsSource, string fsSource)
        {
            var vertexShader = await this.LoadShaderAsync(gl, ShaderType.VERTEX_SHADER, vsSource);
            var fragmentShader = await this.LoadShaderAsync(gl, ShaderType.FRAGMENT_SHADER, fsSource);

            var program = await gl.CreateProgramAsync();
            await gl.EnableAsync(EnableCap.BLEND);
            await gl.BlendFuncAsync(BlendingMode.SRC_ALPHA, BlendingMode.ONE_MINUS_SRC_ALPHA);
            await gl.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_S, (float)TextureParameterValue.CLAMP_TO_EDGE);
            await gl.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_T, (float)TextureParameterValue.CLAMP_TO_EDGE);
            await gl.AttachShaderAsync(program, vertexShader);
            await gl.AttachShaderAsync(program, fragmentShader);
            await gl.LinkProgramAsync(program);
            await gl.DeleteShaderAsync(vertexShader);
            await gl.DeleteShaderAsync(fragmentShader);

            if (!await gl.GetProgramParameterAsync<bool>(program, ProgramParameter.LINK_STATUS))
            {
                string info = await gl.GetProgramInfoLogAsync(program);
                throw new Exception("An error occured while linking the program: " + info);
            }

            u_matrix_location = await _context.GetUniformLocationAsync(program, "u_matrix");

            return program;
        }

        private async Task<WebGLShader> LoadShaderAsync(WebGLContext gl, ShaderType type, string source)
        {
            var shader = await gl.CreateShaderAsync(type);

            await gl.ShaderSourceAsync(shader, source);
            await gl.CompileShaderAsync(shader);

            if (!await gl.GetShaderParameterAsync<bool>(shader, ShaderParameter.COMPILE_STATUS))
            {
                string info = await gl.GetShaderInfoLogAsync(shader);
                await gl.DeleteShaderAsync(shader);
                throw new Exception("An error occured while compiling the shader: " + info);
            }

            return shader;
        }

        public async Task RenderFrame()
        {
            // Enqueue all gameobjects to be rendered
            IEnumerable<GameObject> obs =
                GameObjectTransport.Instance.GameState.gameObjects.Values.OrderBy(g => g.GetRenderOrder()).TakeLast(MAX_GAME_OBJECTS);
            foreach (GameObject gameObject in obs)
            {
                QueueGameObject(gameObject);
            }

            // Clear out any remaining unset vertices
            for (int i = gameObjectCount * 6 * 6; i < vertices.Length; i++) vertices[i] = 0;

            await _context.BeginBatchAsync();

            await _context.ClearColorAsync(0, 0, 0, 1);

            await _context.ClearAsync(BufferBits.COLOR_BUFFER_BIT);

            Buffer.BlockCopy(vertices, 0, byteArr, 0, byteArr.Length);
            await _context.BufferDataAsync(BufferType.ARRAY_BUFFER, byteArr, BufferUsageHint.DYNAMIC_DRAW);

            // this matrix will convert from pixels to clip space
            Matrix4x4 myMatrix = Matrix4x4.CreateOrthographic(_canvasReference.Width, _canvasReference.Height, 1, -1);

            // this matrix will translate our quad to dstX, dstY
            Matrix4x4 transMatrix = Matrix4x4.CreateTranslation(transVector);
            myMatrix = Matrix4x4.Multiply(myMatrix, transMatrix);

            // Set the matrix.
            await _context.UniformMatrixAsync(u_matrix_location, false, new float[16] { myMatrix.M11, myMatrix.M12, myMatrix.M13, myMatrix.M14, myMatrix.M21, myMatrix.M22, myMatrix.M23, myMatrix.M24, myMatrix.M31, myMatrix.M32, myMatrix.M33, myMatrix.M34, myMatrix.M41, myMatrix.M42, myMatrix.M43, myMatrix.M44 });

            await _context.DrawArraysAsync(Primitive.TRIANGLES, 0, 6 * gameObjectCount);

            await _context.EndBatchAsync();

            // Clear all enqueued gameobjects
            gameObjectCount = 0;
        }

        public void QueueGameObject(GameObject gameObject)
        {
            // We want to translate gameObject from world space to screen space

            // Calculate gameobject position in screen space (pixels)
            (float x, float y) centerScreen = (
                (float)(gameObject.Position.x + 0.5f) * PIXELS_PER_UNIT,
                canvasHeight - (float)(gameObject.Position.y + 0.5f) * PIXELS_PER_UNIT);

            // Calculate the half of gameobjects size expressed in screen spacec
            float halfSizeScreen = (float)gameObject.Size * 0.5f * PIXELS_PER_UNIT;


            float myXupLeft = centerScreen.x - halfSizeScreen;
            float myYupLeft = centerScreen.y + halfSizeScreen;

            float myXupRight = centerScreen.x + halfSizeScreen;
            float myYupRight = centerScreen.y + halfSizeScreen;

            float myXdownLeft = centerScreen.x - halfSizeScreen;
            float myYdownLeft = centerScreen.y - halfSizeScreen;

            float myXdownRight = centerScreen.x + halfSizeScreen;
            float myYdownRight = centerScreen.y - halfSizeScreen;

            vertices[0 + (gameObjectCount * 6 * 6)] = myXdownLeft;
            vertices[1 + (gameObjectCount * 6 * 6)] = myYdownLeft;
            vertices[2 + (gameObjectCount * 6 * 6)] = 0;

            vertices[3 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[4 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            vertices[6 + (gameObjectCount * 6 * 6)] = myXdownRight;
            vertices[7 + (gameObjectCount * 6 * 6)] = myYdownRight;
            vertices[8 + (gameObjectCount * 6 * 6)] = 0;

            vertices[9 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[10 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            vertices[12 + (gameObjectCount * 6 * 6)] = myXupLeft;
            vertices[13 + (gameObjectCount * 6 * 6)] = myYupLeft;
            vertices[14 + (gameObjectCount * 6 * 6)] = 0;

            vertices[15 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[16 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            vertices[18 + (gameObjectCount * 6 * 6)] = myXupLeft;
            vertices[19 + (gameObjectCount * 6 * 6)] = myYupLeft;
            vertices[20 + (gameObjectCount * 6 * 6)] = 0;

            vertices[21 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[22 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            vertices[24 + (gameObjectCount * 6 * 6)] = myXupRight;
            vertices[25 + (gameObjectCount * 6 * 6)] = myYupRight;
            vertices[26 + (gameObjectCount * 6 * 6)] = 0;

            vertices[27 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[28 + (gameObjectCount * 6 * 6)] = (0.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            vertices[30 + (gameObjectCount * 6 * 6)] = myXdownRight;
            vertices[31 + (gameObjectCount * 6 * 6)] = myYdownRight;
            vertices[32 + (gameObjectCount * 6 * 6)] = 0;

            vertices[33 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.x) * SPRITE_SIZE_IN_SHEET;
            vertices[34 + (gameObjectCount * 6 * 6)] = (1.0f + gameObject.SpriteIndex.y) * SPRITE_SIZE_IN_SHEET;

            gameObjectCount++;
        }
    }
}
