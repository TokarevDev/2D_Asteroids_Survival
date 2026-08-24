using Game.Core.Physics;
using Game.Core.Weapons;
using Game.Gameplay.Enemies;
using Game.Gameplay.Player;
using Game.Gameplay.Weapons;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class LaserWeaponInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LaserChargeMagazine>().AsSingle();

            Container.BindInterfacesTo<LaserRechargeController>().AsSingle().NonLazy();

            Container.Bind<SegmentCircleIntersectionDetector2D>().AsSingle();

            Container.Bind<LaserTargetQuery>().AsSingle();

            Container.Bind<EnemyDestructionService>().AsSingle();

            Container.Bind<LaserShotService>().AsSingle().NonLazy();

            Container.Bind<WeaponOrigin>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerLaserWeapon>().AsSingle().NonLazy();

            Container.Bind<LaserView>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<LaserViewController>().AsSingle().NonLazy();
        }
    }
}
