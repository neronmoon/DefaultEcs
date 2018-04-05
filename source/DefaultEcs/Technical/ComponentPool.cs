﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultEcs.Technical.Message;

namespace DefaultEcs.Technical
{
    internal sealed class ComponentPool<T>
    {
        #region Fields

        private readonly int[] _mapping;
        private readonly int[] _refCount;
        private readonly int[] _reverseMapping;
        private readonly T[] _items;

        private int _lastIndex;

        #endregion

        #region Initialisation

        public ComponentPool(int maxEntityCount, int maxComponentCount)
        {
            _mapping = Enumerable.Repeat(-1, maxEntityCount).ToArray();
            _refCount = new int[maxEntityCount];
            _reverseMapping = new int[maxComponentCount];
            _items = new T[maxComponentCount];

            _lastIndex = -1;
        }

        #endregion

        #region Callbacks

        public void On(in EntityDisposedMessage message) => Remove(message.Entity.EntityId);

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entityId) => _mapping[entityId] != -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Set(int entityId, in T item)
        {
            ref int index = ref _mapping[entityId];
            if (index != -1)
            {
                _items[index] = item;

                return false;
            }

            if (_lastIndex == _items.Length - 1)
            {
                throw new InvalidOperationException($"Max number of component of type {nameof(T)} reached");
            }

            _items[++_lastIndex] = item;
            index = _lastIndex;
            _reverseMapping[_lastIndex] = entityId;
            ++_refCount[_lastIndex];

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetSameAs(int entityId, int refId)
        {
            int refIndex = _mapping[refId];

            bool isNew = true;
            ref int index = ref _mapping[entityId];
            if (index != -1)
            {
                if (index == refIndex)
                {
                    return false;
                }

                Remove(entityId);
                isNew = false;
            }

            if (_reverseMapping[refIndex] == refId)
            {
                ++_refCount[refIndex];
            }

            index = refIndex;

            return isNew;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(int entityId)
        {
            ref int index = ref _mapping[entityId];
            if (index == -1)
            {
                return false;
            }

            if (--_refCount[index] == 0)
            {
                if (index != _lastIndex)
                {
                    int lastId = _reverseMapping[_lastIndex];
                    _items[index] = _items[_lastIndex];
                    _refCount[index] = _refCount[_lastIndex];
                    _refCount[_lastIndex] = 0;
                    _mapping[lastId] = index;
                    _reverseMapping[index] = lastId;
                }

                --_lastIndex;
            }
            else
            {
                int refIndex = index;
                for (int i = 0; i < _mapping.Length; ++i)
                {
                    if (_mapping[i] == refIndex && i != entityId)
                    {
                        _reverseMapping[index] = i;
                        break;
                    }
                }
            }

            index = -1;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId)
        {
            int index = _mapping[entityId];
            if (index == -1 || index > _lastIndex)
            {
                throw new InvalidOperationException($"Entity does not have a component of type {nameof(T)}");
            }

            return ref _items[_mapping[entityId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> GetAll() => new Span<T>(_items, 0, _lastIndex + 1);

        #endregion
    }
}