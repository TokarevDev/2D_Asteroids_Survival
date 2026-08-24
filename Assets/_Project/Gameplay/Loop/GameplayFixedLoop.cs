using System;
using Game.Gameplay.Enemies;
using Game.Gameplay.Enemies.Ufo;
using Game.Gameplay.Physics;
using Game.Gameplay.Player;
using Game.Gameplay.Projectiles;
using Zenject;

namespace Game.Gameplay.Loop
{
    public sealed class GameplayFixedLoop : IFixedTickable
    {
        private readonly PlayerInvulnerabilityController _playerInvulnerability;
        private readonly PlayerPhysicsController _playerPhysics;
        private readonly UfoPursuitController _ufoPursuit;
        private readonly CustomPhysicsFixedTickRunner _physicsRunner;
        private readonly ProjectileLifetimeController _projectileLifetime;
        private readonly ProjectileWorldExitController _projectileWorldExit;
        private readonly PlayerWorldWrapController _playerWorldWrap;
        private readonly EnemyWorldWrapController _enemyWorldWrap;
        private readonly PlayerEnemyCollisionController _playerCollision;
        private readonly ProjectileEnemyCollisionController _projectileCollision;
        private readonly PlayerPhysicsViewSynchronizer _playerViewSynchronizer;
        private readonly EnemyPhysicsViewSynchronizer _enemyViewSynchronizer;
        private readonly ProjectilePhysicsViewSynchronizer _projectileViewSynchronizer;

        public GameplayFixedLoop(
            PlayerInvulnerabilityController playerInvulnerability,
            PlayerPhysicsController playerPhysics,
            UfoPursuitController ufoPursuit,
            CustomPhysicsFixedTickRunner physicsRunner,
            ProjectileLifetimeController projectileLifetime,
            ProjectileWorldExitController projectileWorldExit,
            PlayerWorldWrapController playerWorldWrap,
            EnemyWorldWrapController enemyWorldWrap,
            PlayerEnemyCollisionController playerCollision,
            ProjectileEnemyCollisionController projectileCollision,
            PlayerPhysicsViewSynchronizer playerViewSynchronizer,
            EnemyPhysicsViewSynchronizer enemyViewSynchronizer,
            ProjectilePhysicsViewSynchronizer projectileViewSynchronizer)
        {
            _playerInvulnerability = playerInvulnerability
                                     ?? throw new ArgumentNullException(nameof(playerInvulnerability));
            _playerPhysics = playerPhysics
                             ?? throw new ArgumentNullException(nameof(playerPhysics));
            _ufoPursuit = ufoPursuit
                          ?? throw new ArgumentNullException(nameof(ufoPursuit));
            _physicsRunner = physicsRunner
                             ?? throw new ArgumentNullException(nameof(physicsRunner));
            _projectileLifetime = projectileLifetime
                                  ?? throw new ArgumentNullException(nameof(projectileLifetime));
            _projectileWorldExit = projectileWorldExit
                                   ?? throw new ArgumentNullException(nameof(projectileWorldExit));
            _playerWorldWrap = playerWorldWrap
                               ?? throw new ArgumentNullException(nameof(playerWorldWrap));
            _enemyWorldWrap = enemyWorldWrap
                              ?? throw new ArgumentNullException(nameof(enemyWorldWrap));
            _playerCollision = playerCollision
                               ?? throw new ArgumentNullException(nameof(playerCollision));
            _projectileCollision = projectileCollision
                                   ?? throw new ArgumentNullException(nameof(projectileCollision));
            _playerViewSynchronizer = playerViewSynchronizer
                                      ?? throw new ArgumentNullException(nameof(playerViewSynchronizer));
            _enemyViewSynchronizer = enemyViewSynchronizer
                                     ?? throw new ArgumentNullException(nameof(enemyViewSynchronizer));
            _projectileViewSynchronizer = projectileViewSynchronizer
                                          ?? throw new ArgumentNullException(nameof(projectileViewSynchronizer));
        }

        public void FixedTick()
        {
            RunPlayerStateStage();
            RunMovementStage();
            RunPhysicsIntegrationStage();
            RunProjectileLifecycleStage();
            RunWorldBoundaryStage();
            RunCollisionStage();
            RunPresentationStage();
        }

        private void RunPlayerStateStage()
        {
            _playerInvulnerability.FixedTick();
        }

        private void RunMovementStage()
        {
            _playerPhysics.FixedTick();
            _ufoPursuit.FixedTick();
        }

        private void RunPhysicsIntegrationStage()
        {
            _physicsRunner.FixedTick();
        }

        private void RunProjectileLifecycleStage()
        {
            _projectileLifetime.FixedTick();
            _projectileWorldExit.FixedTick();
        }

        private void RunWorldBoundaryStage()
        {
            _playerWorldWrap.FixedTick();
            _enemyWorldWrap.FixedTick();
        }

        private void RunCollisionStage()
        {
            _playerCollision.FixedTick();
            _projectileCollision.FixedTick();
        }

        private void RunPresentationStage()
        {
            _playerViewSynchronizer.FixedTick();
            _enemyViewSynchronizer.FixedTick();
            _projectileViewSynchronizer.FixedTick();
        }
    }
}
