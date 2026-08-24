using System;
using Game.Gameplay.Asteroids;
using Game.Gameplay.Enemies.Ufo;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class EnemySpawningInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidConfig[] _asteroidConfigs;

        public override void InstallBindings()
        {
            ValidateSerializedReferences();

            AsteroidConfigSelector configSelector = new AsteroidConfigSelector(_asteroidConfigs);

            Container.Bind<AsteroidConfigSelector>()
                .FromInstance(configSelector)
                .AsSingle();

            Container.Bind<AsteroidSpawnAction>().AsSingle();

            Container.BindInterfacesTo<AsteroidSpawner>()
                .AsSingle()
                .NonLazy();

            Container.Bind<UfoSpawnAction>().AsSingle();

            Container.BindInterfacesTo<UfoSpawner>()
                .AsSingle()
                .NonLazy();
        }

        private void ValidateSerializedReferences()
        {
            if (_asteroidConfigs == null || _asteroidConfigs.Length == 0)
            {
                throw new InvalidOperationException(
                    "At least one asteroid config must be assigned");
            }

            for (int i = 0; i < _asteroidConfigs.Length; i++)
            {
                if (_asteroidConfigs[i] == null)
                {
                    throw new InvalidOperationException(
                        $"Asteroid config at index {i} is missing");
                }
            }
        }
    }
}
