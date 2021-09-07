using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bomberman.Client.Audio
{
    public interface ISoundLoader
    {
        IEnumerable<SoundEffect> AllSounds { get; }
        ValueTask LoadAll(IJSRuntime runtime);
    }
}
