using Bomberman.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bomberman.Client.Audio
{
    public class SoundLoader : ISoundLoader
    {
        public Dictionary<SoundName, SoundEffect> sounds = new Dictionary<SoundName, SoundEffect>();
        public IEnumerable<SoundEffect> AllSounds => sounds.Values;

        public async ValueTask LoadAll(IJSRuntime runtime)
        {
            sounds = new Dictionary<SoundName, SoundEffect>
            {
                [SoundName.BombExplosion] =
                    await loadFile(SoundName.BombExplosion, "assets/audio/bombexplosion.wav"),
                [SoundName.PowerupPickup] = await loadFile(SoundName.PowerupPickup, "assets/audio/powerup.wav"),
                [SoundName.PlayerDies] = await loadFile(SoundName.PlayerDies, "assets/audio/playerdying.wav"),
                [SoundName.EnemyDies] = await loadFile(SoundName.PlayerDies, "assets/audio/enemydying.wav")
            };

            // ReSharper disable once HeapView.ClosureAllocation
            async ValueTask<SoundEffect> loadFile(SoundName name, string path)
            {
                var s = name.ToString();
                await runtime.InvokeAsync<object>("soundPlayer.loadSound", new object[] { s, path });

                return new SoundEffect(runtime, s);
            }
        }
    }
}
