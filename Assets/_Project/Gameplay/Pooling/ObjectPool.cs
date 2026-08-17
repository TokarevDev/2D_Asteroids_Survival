using System;
using System.Collections.Generic;

namespace Game.Gameplay.Pooling
{
    public sealed class ObjectPool<T> where T : class
    {
        private readonly Func<T> _createItem;
        private readonly List<T> _createdItems;
        private readonly Queue<T> _availableItems;
        private readonly HashSet<T> _availableItemSet;

        public ObjectPool(Func<T> createItem, int initialCapacity)
        {
            _createItem = createItem ?? throw new ArgumentNullException(nameof(createItem));

            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _createdItems = new List<T>(initialCapacity);
            _availableItems = new Queue<T>(initialCapacity);
            _availableItemSet = new HashSet<T>();

            Prewarm(initialCapacity);
        }

        public IReadOnlyList<T> CreatedItems
        {
            get { return _createdItems; }
        }

        public T Get()
        {
            if (_availableItems.Count == 0)
            {
                return CreateItem();
            }

            T item = _availableItems.Dequeue();
            _availableItemSet.Remove(item);

            return item;
        }

        public bool Return(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_availableItemSet.Add(item))
            {
                return false;
            }

            _availableItems.Enqueue(item);
            return true;
        }

        public void Clear()
        {
            _createdItems.Clear();
            _availableItems.Clear();
            _availableItemSet.Clear();
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T item = CreateItem();
                Return(item);
            }
        }

        private T CreateItem()
        {
            T item = _createItem();

            if (item == null)
            {
                throw new InvalidOperationException("Object pool factory returned null");
            }

            _createdItems.Add(item);
            return item;
        }
    }
}
