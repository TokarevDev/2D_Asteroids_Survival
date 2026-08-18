using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Configuration;
using Game.Core.Scenes;
using Game.Infrastructure.Analytics;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.Bootstrap
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;
        private IGameConfigLoader _gameConfigLoader;
        private FirebaseInitializer _firebaseInitializer;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, IGameConfigLoader gameConfigLoader,
            FirebaseInitializer firebaseInitializer)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

            _gameConfigLoader = gameConfigLoader ?? throw new ArgumentNullException(nameof(gameConfigLoader));

            _firebaseInitializer = firebaseInitializer ?? throw new ArgumentNullException(nameof(firebaseInitializer));
        }

        private void Start()
        {
            CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
            BootstrapAsync(cancellationToken).Forget(Debug.LogException);
        }

        private async UniTask BootstrapAsync(CancellationToken cancellationToken)
        {
            await _gameConfigLoader.LoadAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _firebaseInitializer.InitializeAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _sceneLoader.LoadMainMenuAsync();
        }
    }
}
