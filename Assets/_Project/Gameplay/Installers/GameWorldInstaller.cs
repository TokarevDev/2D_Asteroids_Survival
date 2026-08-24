using Game.Core.World;
using Game.Gameplay.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class GameWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ToroidalWorld2D>().AsSingle();

            Container.Bind<RandomWorldSpawnPointProvider>().AsSingle();

            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();

            Container.Bind<CameraProvider>().AsSingle();

            Container.BindInterfacesTo<CameraWorldBoundsSynchronizer>().AsSingle().NonLazy();
        }
    }
}
