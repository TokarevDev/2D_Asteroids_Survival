using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            BootstrapAsync().Forget(Debug.LogException);
        }

        private async UniTask BootstrapAsync()
        {
            await _sceneLoader.LoadMainMenuAsync();
        }
    }
}
