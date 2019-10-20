using UniFlow.Signal;
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
        }
    }
}
