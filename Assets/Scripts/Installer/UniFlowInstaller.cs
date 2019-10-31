using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UniFlow.Signal;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Installer
{
    [CreateAssetMenu(menuName = "Installers/UniFlowInstaller", fileName = "UniFlowInstaller")]
    public class UniFlowInstaller : ScriptableObjectInstaller<UniFlowInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareUniFlowSignal<StringSignal>();
            Container.DeclareUniFlowSignal<HandleEventSignal>();
            Container.DeclareUniFlowSignal<MusicControlSignal>();
            Container.DeclareUniFlowSignal<MusicDuckingSignal>();
            Container.DeclareUniFlowSignal<MusicPitchSignal>();
        }

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        private void AOTWorkaround()
        {
            new SignalHandler<StringSignal>();
            new SignalHandler<HandleEventSignal>();
            new SignalHandler<MusicControlSignal>();
            new SignalHandler<MusicDuckingSignal>();
            new SignalHandler<MusicPitchSignal>();
        }
    }
}
