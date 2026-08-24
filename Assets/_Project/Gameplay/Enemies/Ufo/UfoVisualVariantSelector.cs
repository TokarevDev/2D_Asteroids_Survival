using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Enemies.Ufo
{
    [DisallowMultipleComponent]
    public sealed class UfoVisualVariantSelector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _variants;

        public bool IsConfigured =>
            _spriteRenderer != null &&
            _variants != null &&
            _variants.Length > 0;

        public void ApplyRandomVariant()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException("UFO visual variants are not configured");
            }

            int variantIndex = Random.Range(0, _variants.Length);
            Sprite variant = _variants[variantIndex];

            if (variant == null)
            {
                throw new InvalidOperationException($"UFO visual variant at index {variantIndex} is missing");
            }

            _spriteRenderer.sprite = variant;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            if (_spriteRenderer == null)
            {
                throw new InvalidOperationException("UFO sprite renderer is missing");
            }

            _spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
