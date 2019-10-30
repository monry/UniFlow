namespace UniFlow.Signal
{
    public class MusicDuckingSignal : SignalBase<MusicDuckingSignal>
    {
        public float Volume { get; private set; }
        public float Duration { get; private set; }

        public static MusicDuckingSignal Create(float volume, float duration)
        {
            return new MusicDuckingSignal
            {
                Volume = volume,
                Duration = duration,
            };
        }
    }
}
