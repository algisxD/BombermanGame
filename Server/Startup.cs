using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Bomberman.Server.Hubs;
using Newtonsoft.Json;
using Bomberman.Shared;
using Bomberman.Client;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace Bomberman.Server
{
    public class Startup
    {
        public static GameState GameState { get; private set; }
        public static Chatroom Chatroom { get; private set; }
        public static Queue<Vector2d> FreePlayerSlots = new Queue<Vector2d>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            GameState = new GameState();
            Chatroom = new Chatroom();

            string[] input = File.ReadAllLines(@"map1.txt");
            int[,] map = new int[input.Length, input.Length];
            int i = 0, j = 0;
            foreach (string row in input)
            {
                j = 0;
                foreach (char ch in row)
                {
                    map[i, j] = int.Parse(ch.ToString());
                    j++;
                }
                i++;
            }

            World.TilesCountInOneDimention = input.Length;
            double xPos = 0, yPos = 0;
            for (int x = 0; x < input.Length; x++)
            {
                xPos = 0;
                for (int y = 0; y < input.Length; y++)
                {
                    GameObject.Type type =(GameObject.Type)map[x, y];

                    GameObject tile = new GameObject
                    {
                        type = type,
                        Size = World.TileSize,
                        Position = new Vector2d(xPos, yPos)
                    };
                    GameState.Apply(tile);

                    if (type == GameObject.Type.Player)
                    {
                        FreePlayerSlots.Enqueue(new Vector2d(xPos, yPos));
                    }

                    // TODO: handle this better
                    if (tile.type == GameObject.Type.Player)
                    {
                        tile.type = GameObject.Type.Ground;
                    }

                    xPos += World.TileSize;
                }
                yPos += World.TileSize;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers().AddNewtonsoftJson();
            //services.AddResponseCompression(opts =>
            //{
            //    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            //        new[] { "application/octet-stream" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameObjectHub>("/gameobjecthub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
