namespace UniFlow.Signal
{
    public class MusicPitchSignal : SignalBase<MusicPitchSignal>
    {
        public float Pitch { get; private set; }
        public float Duration { get; private set; }

        public static MusicPitchSignal Create(float volume, float duration)
        {
            return new MusicPitchSignal
            {
                Pitch = volume,
                Duration = duration,
            };
        }
    }
}
