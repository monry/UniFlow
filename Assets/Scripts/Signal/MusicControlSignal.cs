using UniFlow.Connector.Controller;
using UnityEngine;

namespace UniFlow.Signal
{
    public class MusicControlSignal : SignalBase<MusicControlSignal>
    {
        public AudioControlMethod AudioControlMethod { get; private set; }
        public AudioClip AudioClip { get; private set; }
        public bool Loop { get; private set; } = true;
        public bool RewindSameClip { get; private set; } = false;

        public static MusicControlSignal Create(AudioControlMethod musicControlType, AudioClip audioClip, bool loop = true, bool rewindSameClip = false)
        {
            return new MusicControlSignal
            {
                AudioControlMethod = musicControlType,
                AudioClip = audioClip,
                Loop = loop,
                RewindSameClip = rewindSameClip,
            };
        }
    }
}
