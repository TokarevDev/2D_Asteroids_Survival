using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using Game.Gameplay.Pooling;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoPool : MonoBehaviour
    {
        public event Action<EnemyType, DeathSource> UfoDied;

        private const int UfoSortingOrderBase = 200;

        [SerializeField] private Ufo _ufoPrefab;

        private EnemyLifecycleService _enemyLifecycleService;
        private IGameConfigProvider _configProvider;

        private ObjectPool<Ufo> _pool;
        private int _nextSortingOrder = UfoSortingOrderBase;

        [Inject]
        private void Construct(EnemyLifecycleService enemyLifecycleService, IGameConfigProvider configProvider)
        {
            _enemyLifecycleService =
                enemyLifecycleService ?? throw new ArgumentNullException(nameof(enemyLifecycleService));

            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Ufo>(CreateUfo, _configProvider.World.InitialUfoPoolSize);
        }

        private void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Ufo ufo = _pool.CreatedItems[i];

                if (ufo == null)
                {
                    continue;
                }

                ufo.Died -= OnUfoDied;
            }

            _pool.Clear();
        }

        public Ufo Get(Vector2 position, Vector2 velocity)
        {
            EnemyParameters parameters = _configProvider.Enemy.GetParameters(EnemyType.Ufo);

            Ufo ufo = _pool.Get();

            try
            {
                ufo.transform.SetPositionAndRotation(position, Quaternion.identity);

                ufo.Initialize(parameters.MaxHealth);

                _enemyLifecycleService.Spawn(ufo.PhysicsView, EnemyType.Ufo, position, velocity, 0f);
            }
            catch
            {
                Return(ufo);
                throw;
            }

            ufo.gameObject.SetActive(true);
            return ufo;
        }

        public void Return(Ufo ufo)
        {
            if (ufo == null)
            {
                Debug.LogError("Cannot return a null UFO", this);
                return;
            }

            if (ufo.PhysicsView.IsBound && !_enemyLifecycleService.Despawn(ufo.PhysicsView))
            {
                Debug.LogError("Bound UFO physics view was not active", ufo);
            }

            if (!_pool.Return(ufo))
            {
                Debug.LogWarning("UFO is already in the pool", ufo);
                return;
            }

            ufo.gameObject.SetActive(false);
        }

        public bool TryGetByEntity(EnemyEntity entity, out Ufo ufo)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Ufo candidate = _pool.CreatedItems[i];

                if (candidate == null || !candidate.PhysicsView.IsBound)
                {
                    continue;
                }

                if (ReferenceEquals(candidate.PhysicsView.Entity, entity))
                {
                    ufo = candidate;
                    return true;
                }
            }

            ufo = null;
            return false;
        }

        private Ufo CreateUfo()
        {
            Ufo ufo = Instantiate(_ufoPrefab, transform);

            ufo.SetSortingOrder(_nextSortingOrder);
            _nextSortingOrder++;

            ufo.Died += OnUfoDied;
            ufo.gameObject.SetActive(false);

            return ufo;
        }

        private void OnUfoDied(Ufo ufo, DeathSource deathSource)
        {
            EnemyType enemyType = ufo.PhysicsView.Entity.Type;

            Return(ufo);

            UfoDied?.Invoke(enemyType, deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            if (_ufoPrefab != null)
            {
                return true;
            }

            Debug.LogError("UFO prefab reference is missing", this);
            return false;
        }
    }
}
