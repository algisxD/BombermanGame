using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bomberman.Client.Audio
{
    public class SoundPlayer
    {
        readonly ISoundLoader _loader;
        readonly IGetSound _getsound;
        bool _loaded;
        bool _enabled;

        public SoundPlayer(ISoundLoader loader, IGetSound getsound)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _getsound = getsound;
            _enabled = true;
        }


        public async ValueTask PowerUp()
        {
            throwIfNotLoaded();

            await play(SoundName.PowerupPickup);
        }

        public async ValueTask BombExplosion()
        {
            throwIfNotLoaded();

            await play(SoundName.BombExplosion);
        }

        public async ValueTask EnemyDies()
        {
            throwIfNotLoaded();

            await play(SoundName.EnemyDies);
        }

        public async ValueTask PlayerDies()
        {
            throwIfNotLoaded();

            await play(SoundName.PlayerDies);
        }

        async ValueTask play(SoundName soundName)
        {
            throwIfNotLoaded();
            
            SoundEffect audio = _getsound.GetSoundEffect(soundName);

            await audio.Play();
        }

        void throwIfNotLoaded()
        {
            if (!_loaded)
            {
                throw new InvalidOperationException("sounds not loaded");
            }
        }
    }
}
