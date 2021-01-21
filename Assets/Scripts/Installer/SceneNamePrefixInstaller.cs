using UnityEngine;
using Zenject;

namespace UniFlow.Installer
{
    public class SceneNamePrefixInstaller : MonoInstaller<SceneNamePrefixInstaller>
    {
        [SerializeField] private string sceneNamePrefix = default;
        private string SceneNamePrefix => sceneNamePrefix;

        public override void InstallBindings()
        {
            Container.BindInstance(SceneNamePrefix).WithId(InjectId.SceneNamePrefix).AsCached();
        }
    }
}