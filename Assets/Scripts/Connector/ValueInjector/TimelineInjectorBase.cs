using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniFlow.Connector.ValueInjector
{
    public abstract class TimelineInjectorBase<TPlayableAsset> : InjectorBase where TPlayableAsset : PlayableAsset
    {
        protected abstract PlayableDirector PlayableDirector { get; set; }
        protected abstract string TrackName { get; set; }
        protected abstract string ClipName { get; set; }

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
                .Where(x => x != default && (string.IsNullOrEmpty(ClipName) || Regex.IsMatch(x.displayName, ClipName)))
                .Select(x => x.asset)
                .OfType<TPlayableAsset>()
                .ToList()
                .ForEach(Inject);
        }

        protected abstract void Inject(TPlayableAsset playableAsset);
    }
}
