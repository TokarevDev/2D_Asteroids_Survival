using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoPool : EnemyPool<Ufo>
    {
        private const int UfoSortingOrderBase = 200;

        [SerializeField] private Ufo _ufoPrefab;

        private IGameConfigProvider _configProvider;

        [Inject]
        private void Construct(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            InitializePool(
                InstantiateUfo,
                _configProvider.World.InitialUfoPoolSize,
                UfoSortingOrderBase);
        }

        protected override EnemyPhysicsView GetPhysicsView(Ufo ufo)
        {
            return ufo.PhysicsView;
        }

        protected override void SetSortingOrder(Ufo ufo, int sortingOrder)
        {
            ufo.SetSortingOrder(sortingOrder);
        }

        protected override void SubscribeToDeath(Ufo ufo)
        {
            ufo.Died += OnUfoDied;
        }

        protected override void UnsubscribeFromDeath(Ufo ufo)
        {
            ufo.Died -= OnUfoDied;
        }

        public Ufo Get(Vector2 position, Vector2 velocity)
        {
            EnemyParameters parameters = _configProvider.Enemy.GetParameters(EnemyType.Ufo);

            Ufo ufo = RentEnemy();

            try
            {
                ufo.Initialize(parameters.MaxHealth);
                ActivateEnemy(ufo, EnemyType.Ufo, position, velocity);
            }
            catch
            {
                Return(ufo);
                throw;
            }

            return ufo;
        }

        private Ufo InstantiateUfo()
        {
            return Instantiate(_ufoPrefab, transform);
        }

        private void OnUfoDied(Ufo ufo, DeathSource deathSource)
        {
            HandleEnemyDeath(ufo, deathSource);
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
