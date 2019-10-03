using System.Linq;
using System.Text.RegularExpressions;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector
{
    public abstract class TimelineInjectorBase<TPlayableAsset> : InjectorBase where TPlayableAsset : PlayableAsset
    {
        [SerializeField] private PlayableDirector playableDirector = default;
        [SerializeField] private string trackName = default;
        [SerializeField] private string clipName = default;

        [ValueReceiver] public PlayableDirector PlayableDirector
        {
            get =>
                playableDirector != default
                    ? playableDirector
                    : playableDirector =
                        BaseGameObject.GetComponent<PlayableDirector>() != default
                            ? BaseGameObject.GetComponent<PlayableDirector>()
                            : BaseGameObject.AddComponent<PlayableDirector>();
            set => playableDirector = value;
        }
        [ValueReceiver] public string TrackName
        {
            get => trackName;
            set => trackName = value;
        }
        [ValueReceiver] public string ClipName
        {
            get => clipName;
            set => clipName = value;
        }

        protected override void Inject()
        {
            if (!(PlayableDirector.playableAsset is TimelineAsset timelineAsset))
            {
                return;
            }

            timelineAsset
                .GetOutputTracks()
                .Where(x => x != default && (string.IsNullOrEmpty(TrackName) || Regex.IsMatch(x.name, TrackName)))
                .SelectMany(x => x.GetClips())
                .Where(x => x != default && (string.IsNullOrEmpty(ClipName) || Regex.IsMatch(x.displayName, TrackName)))
                .Select(x => x.asset)
                .OfType<TPlayableAsset>()
                .ToList()
                .ForEach(Inject);
        }

        protected abstract void Inject(TPlayableAsset playableAsset);
    }
}
