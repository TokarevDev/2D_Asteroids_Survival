using System;
using Game.Core.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [DisallowMultipleComponent, RequireComponent(typeof(MeshRenderer))]
    public sealed class BackgroundLayerParallax : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField, Min(0f)] private float _parallaxStrength = 0.005f;

        private ToroidalWorld2D _world;

        private Material _runtimeMaterial;
        private Vector2 _offset;
        private Vector3 _previousTargetPosition;

        [Inject]
        private void Construct(ToroidalWorld2D world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            _runtimeMaterial = meshRenderer.material;
            _offset = _runtimeMaterial.mainTextureOffset;
            _previousTargetPosition = _target.position;
        }

        private void LateUpdate()
        {
            if (_target == null)
            {
                enabled = false;
                return;
            }

            Vector3 currentTargetPosition = _target.position;
            Vector2 movementDelta = _world.GetShortestDisplacement(_previousTargetPosition, currentTargetPosition);

            _previousTargetPosition = currentTargetPosition;

            _offset.x += movementDelta.x * _parallaxStrength;
            _offset.y += movementDelta.y * _parallaxStrength;

            _offset.x = Mathf.Repeat(_offset.x, 1f);
            _offset.y = Mathf.Repeat(_offset.y, 1f);

            _runtimeMaterial.mainTextureOffset = _offset;
        }

        private void OnDestroy()
        {
            if (_runtimeMaterial != null)
            {
                Destroy(_runtimeMaterial);
            }
        }

        private bool ValidateSerializedReferences()
        {
            if (_target != null)
            {
                return true;
            }

            Debug.LogError("Parallax target reference is missing", this);
            return false;
        }
    }
}
