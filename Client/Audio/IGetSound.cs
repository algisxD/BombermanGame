using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bomberman.Client.Audio
{
    public interface IGetSound
    {
        public SoundEffect GetSoundEffect(SoundName name);
    }
}
