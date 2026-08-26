using System;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using Game.Gameplay.Pooling;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies
{
    public abstract class EnemyPool<TEnemy> : MonoBehaviour
        where TEnemy : MonoBehaviour, IDamageable
    {
        private EnemyLifecycleService _lifecycleService;
        private EnemyDeathEventSource _deathEventSource;
        private ObjectPool<TEnemy> _pool;
        private Func<TEnemy> _createEnemy;
        private int _nextSortingOrder;

        [Inject]
        private void Construct(EnemyLifecycleService lifecycleService, EnemyDeathEventSource deathEventSource)
        {
            _lifecycleService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));

            _deathEventSource = deathEventSource ?? throw new ArgumentNullException(nameof(deathEventSource));
        }

        protected void InitializePool(Func<TEnemy> createEnemy, int initialCapacity, int sortingOrderBase)
        {
            if (_pool != null)
            {
                throw new InvalidOperationException(
                    "Enemy pool is already initialized");
            }

            _createEnemy = createEnemy ?? throw new ArgumentNullException(nameof(createEnemy));
            _nextSortingOrder = sortingOrderBase;

            _pool = new ObjectPool<TEnemy>(CreatePooledEnemy, initialCapacity);
        }

        protected TEnemy RentEnemy()
        {
            EnsureInitialized();
            return _pool.Get();
        }

        protected void ActivateEnemy(TEnemy enemy, EnemyType type, Vector2 position, Vector2 velocity)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            EnemyPhysicsView physicsView = GetPhysicsView(enemy);

            if (physicsView == null)
            {
                throw new InvalidOperationException(
                    "Enemy physics view is missing");
            }

            enemy.transform.SetPositionAndRotation(
                position,
                Quaternion.identity);

            _lifecycleService.Spawn(
                physicsView,
                enemy,
                type,
                position,
                velocity,
                0f);

            enemy.gameObject.SetActive(true);
        }

        protected void HandleEnemyDeath(TEnemy enemy, DeathSource deathSource)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            EnemyPhysicsView physicsView = GetPhysicsView(enemy);

            if (physicsView == null)
            {
                throw new InvalidOperationException("Enemy physics view is missing");
            }

            EnemyType enemyType = physicsView.Entity.Type;

            Return(enemy);

            _deathEventSource.Publish(enemyType, deathSource);
        }

        public void Return(TEnemy enemy)
        {
            if (enemy == null)
            {
                Debug.LogError(
                    $"Cannot return a null {typeof(TEnemy).Name}",
                    this);

                return;
            }

            EnsureInitialized();

            EnemyPhysicsView physicsView = GetPhysicsView(enemy);

            if (physicsView.IsBound &&
                !_lifecycleService.Despawn(physicsView))
            {
                Debug.LogError(
                    "Bound enemy physics view was not active",
                    enemy);
            }

            if (!_pool.Return(enemy))
            {
                Debug.LogWarning(
                    "Enemy is already in the pool",
                    enemy);

                return;
            }

            PrepareForReturn(enemy);
            enemy.gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                TEnemy enemy = _pool.CreatedItems[i];

                if (enemy != null)
                {
                    UnsubscribeFromDeath(enemy);
                }
            }

            _pool.Clear();
        }

        protected virtual void PrepareForReturn(TEnemy enemy)
        {
        }

        protected abstract EnemyPhysicsView GetPhysicsView(TEnemy enemy);

        protected abstract void SetSortingOrder(TEnemy enemy, int sortingOrder);

        protected abstract void SubscribeToDeath(TEnemy enemy);

        protected abstract void UnsubscribeFromDeath(TEnemy enemy);

        private TEnemy CreatePooledEnemy()
        {
            TEnemy enemy = _createEnemy();

            if (enemy == null)
            {
                throw new InvalidOperationException(
                    "Enemy factory returned null");
            }

            SetSortingOrder(enemy, _nextSortingOrder);
            _nextSortingOrder++;

            SubscribeToDeath(enemy);
            enemy.gameObject.SetActive(false);

            return enemy;
        }

        private void EnsureInitialized()
        {
            if (_pool == null)
            {
                throw new InvalidOperationException("Enemy pool is not initialized");
            }
        }
    }
}
