﻿using System;
using System.Runtime.CompilerServices;
using DefaultEcs.Technical;
using DefaultEcs.Technical.Message;

namespace DefaultEcs
{
    /// <summary>
    /// Represents an item in the <see cref="WorldId"/>.
    /// Only use <see cref="Entity"/> generated from the <see cref="World.CreateEntity"/> method.
    /// </summary>
    public readonly struct Entity : IDisposable, IEquatable<Entity>
    {
        #region Fields

        internal readonly int WorldId;
        internal readonly int EntityId;

        #endregion

        #region Initialisation

        internal Entity(int worldId, int entityId)
        {
            WorldId = worldId;
            EntityId = entityId;
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfNull()
        {
            if (WorldId == 0)
            {
                throw new InvalidOperationException("Entity was not created from a World");
            }
        }

        /// <summary>
        /// Determines whether the current <see cref="Entity"/> has a component of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <returns>true if the <see cref="Entity"/> has a component of type <typeparamref name="T"/>; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> was not created from a <see cref="WorldId"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>()
        {
            ThrowIfNull();

            return (WorldId < ComponentManager<T>.Pools.Length ? ComponentManager<T>.Pools[WorldId] : null)?.Has(EntityId) ?? false;
        }

        /// <summary>
        /// Sets the value of the component of type <typeparamref name="T"/> on the current <see cref="Entity"/>.
        /// If current <see cref="Entity"/> did not have a component of type <typeparamref name="T"/>, a <see cref="ComponentAddedMessage{T}"/> message is published.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The value of the component.</param>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> was not created from a <see cref="WorldId"/>.</exception>
        /// <exception cref="InvalidOperationException">Max number of component of type <typeparamref name="T"/> reached.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set<T>(in T component)
        {
            ThrowIfNull();

            if (ComponentManager<T>.GetOrCreate(WorldId).Set(EntityId, component))
            {
                ref ComponentEnum components = ref World.EntityComponents[WorldId][EntityId];
                components[ComponentManager<T>.Flag] = true;

                World.Publish(WorldId, new ComponentAddedMessage<T>(this, components));
            }
        }

        /// <summary>
        /// Sets the value of the component of type <typeparamref name="T"/> on the current Entity to the same instance of an other <see cref="Entity"/>.
        /// If current <see cref="Entity"/> did not have a component of type <typeparamref name="T"/>, a <see cref="ComponentAddedMessage{T}"/> message is published.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="reference">The other <see cref="Entity"/> used as reference.</param>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> was not created from a <see cref="WorldId"/>.</exception>
        /// <exception cref="InvalidOperationException">Reference <see cref="Entity"/> comes from a different <see cref="WorldId"/>.</exception>
        /// <exception cref="InvalidOperationException">Reference <see cref="Entity"/> does not have a component of type <typeparamref name="T"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSameAs<T>(in Entity reference)
        {
            ThrowIfNull();

            if (WorldId != reference.WorldId)
            {
                throw new InvalidOperationException("Reference Entity comes from a different World");
            }

            if (!reference.Has<T>())
            {
                throw new InvalidOperationException($"Reference Entity does not have a componenet of type {nameof(T)}");
            }

            if (ComponentManager<T>.Pools[WorldId].SetSameAs(EntityId, reference.EntityId))
            {
                ref ComponentEnum components = ref World.EntityComponents[WorldId][EntityId];
                components[ComponentManager<T>.Flag] = true;

                World.Publish(WorldId, new ComponentAddedMessage<T>(this, components));
            }
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="T"/> on the current <see cref="Entity"/>.
        /// If current <see cref="Entity"/> had a component of type <typeparamref name="T"/>, a <see cref="ComponentRemovedMessage{T}"/> message is published.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <exception cref="InvalidOperationException">Entity was not created from a <see cref="WorldId"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>()
        {
            ThrowIfNull();

            if ((WorldId < ComponentManager<T>.Pools.Length ? ComponentManager<T>.Pools[WorldId] : null)?.Remove(EntityId) ?? false)
            {
                ref ComponentEnum components = ref World.EntityComponents[WorldId][EntityId];
                components[ComponentManager<T>.Flag] = false;
                World.Publish(WorldId, new ComponentRemovedMessage<T>(this, components));
            }
        }

        /// <summary>
        /// Gets the component of type <typeparamref name="T"/> on the current <see cref="Entity"/>.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <returns>A reference to the component.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> was not created from a <see cref="WorldId"/>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> does not have a component of type <typeparamref name="T"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>()
        {
            ThrowIfNull();

            if (WorldId >= ComponentManager<T>.Pools.Length
                || ComponentManager<T>.Pools[WorldId] == null)
            {
                throw new InvalidOperationException($"Entity does not have a component of type {nameof(T)}");
            }

            return ref ComponentManager<T>.Pools[WorldId].Get(EntityId);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Clean the current <see cref="Entity"/> of all its components and a <see cref="EntityDisposedMessage"/> message is published.
        /// The current <see cref="Entity"/> should not be used again after calling this method.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Entity"/> was not created from a <see cref="WorldId"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ThrowIfNull();

            World.Publish(WorldId, new EntityDisposedMessage(this));
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(Entity other) => EntityId == other.EntityId;

        #endregion

        #region Object

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value;
        /// otherwise, false.
        /// </returns>
        public override bool Equals(object obj) => obj is Entity entity ? Equals(entity) : false;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() => EntityId;

        #endregion
    }
}
