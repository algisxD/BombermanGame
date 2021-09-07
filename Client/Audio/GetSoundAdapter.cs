using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bomberman.Client.Audio
{
    public class GetSoundAdapter : IGetSound
    {
        private readonly SoundLoader _adaptee;

        public GetSoundAdapter(SoundLoader adaptee)
        {
            this._adaptee = adaptee;
        }
        public SoundEffect GetSoundEffect(SoundName name)
        {
            SoundEffect effect = this._adaptee.sounds.Where(s => s.Key == name).FirstOrDefault().Value;

            return effect;
        }
    }
}
