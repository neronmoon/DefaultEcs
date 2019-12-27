using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultEcs.Technical;
using DefaultEcs.Technical.Message;

namespace DefaultEcs {
    /// <inheritdoc />
    public class EntityMutator : IEntityMutator {
        
        /// <inheritdoc />
        public void SetDisabled<T>(Entity entity, in T component) => ComponentManager<T>.GetOrCreate(entity.WorldId).Set(entity.EntityId, component);

        /// <inheritdoc />
        public void SetSameAsDisabled<T>(Entity entity, in Entity reference)
        {
            ComponentPool<T> pool = ComponentManager<T>.Get(entity.WorldId);
            if (!(pool?.Has(reference.EntityId) ?? false)) Throw($"Reference Entity does not have a component of type {nameof(T)}");

            pool?.SetSameAs(entity.EntityId, reference.EntityId);
        }

        /// <inheritdoc />
        public void Enable(Entity entity) {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            ref ComponentEnum components = ref GetComponentsReference(entity);
            if (!components[World.IsEnabledFlag]) {
                components[World.IsEnabledFlag] = true;
                Publisher.Publish(entity.WorldId, new EntityEnabledMessage(entity.EntityId, components));
            }
        }

        /// <inheritdoc />
        public void Disable(Entity entity) {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            ref ComponentEnum components = ref GetComponentsReference(entity);
            if (components[World.IsEnabledFlag]) {
                components[World.IsEnabledFlag] = false;
                Publisher.Publish(entity.WorldId, new EntityDisabledMessage(entity.EntityId, components));
            }
        }

        /// <inheritdoc />
        public void Enable<T>(Entity entity) {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            if (entity.Has<T>()) {
                ref ComponentEnum components = ref GetComponentsReference(entity);
                if (!components[ComponentManager<T>.Flag]) {
                    components[ComponentManager<T>.Flag] = true;
                    Publisher.Publish(entity.WorldId, new ComponentAddedMessage<T>(entity.EntityId, components));
                }
            }
        }

        /// <inheritdoc />
        public void Disable<T>(Entity entity) {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            ref ComponentEnum components = ref GetComponentsReference(entity);
            if (components[ComponentManager<T>.Flag]) {
                components[ComponentManager<T>.Flag] = false;
                Publisher.Publish(entity.WorldId, new ComponentRemovedMessage<T>(entity.EntityId, components));
            }
        }

        /// <inheritdoc />
        public void Set<T>(Entity entity, in T component) {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            ref ComponentEnum components = ref GetComponentsReference(entity);
            if (ComponentManager<T>.GetOrCreate(entity.WorldId).Set(entity.EntityId, component)) {
                components[ComponentManager<T>.Flag] = true;
                Publisher.Publish(entity.WorldId, new ComponentAddedMessage<T>(entity.EntityId, components));
            } else if (components[ComponentManager<T>.Flag]) {
                Publisher.Publish(entity.WorldId, new ComponentChangedMessage<T>(entity.EntityId, components));
            }
        }

        /// <inheritdoc />
        public void SetSameAs<T>(Entity target, in Entity reference) {
            if (target.WorldId == 0) Throw("Entity was not created from a World");
            if (target.WorldId != reference.World.WorldId) Throw("Reference Entity comes from a different World");
            ComponentPool<T> pool = ComponentManager<T>.Get(target.WorldId);
            if (!(pool?.Has(reference.EntityId) ?? false))
                Throw($"Reference Entity does not have a component of type {nameof(T)}");

            ref ComponentEnum components = ref GetComponentsReference(target);
            if (pool.SetSameAs(target.EntityId, reference.EntityId)) {
                components[ComponentManager<T>.Flag] = true;
                Publisher.Publish(target.WorldId, new ComponentAddedMessage<T>(target.EntityId, components));
            } else if (components[ComponentManager<T>.Flag]) {
                Publisher.Publish(target.WorldId, new ComponentChangedMessage<T>(target.EntityId, components));
            }
        }

        /// <inheritdoc />
        public void Remove<T>(Entity entity) {
            if (ComponentManager<T>.Get(entity.WorldId)?.Remove(entity.EntityId) ?? false) {
                ref ComponentEnum components = ref GetComponentsReference(entity);
                components[ComponentManager<T>.Flag] = false;
                Publisher.Publish(entity.WorldId, new ComponentRemovedMessage<T>(entity.EntityId, components));
            }
        }


        /// <inheritdoc />
        public void SetAsChildOf(Entity entity, in Entity parent) {
            if (entity.WorldId != parent.WorldId) Throw("Child and parent Entity come from a different World");
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            ref HashSet<int> children = ref entity.World.EntityInfos[parent.EntityId].Children;
            children ??= new HashSet<int>();

            if (children.Add(entity.EntityId)) {
                entity.World.EntityInfos[entity.EntityId].Parents += children.Remove;
            }
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAsParentOf(Entity parent, in Entity child) => child.SetAsChildOf(parent);

        /// <inheritdoc />
        public void RemoveFromChildrenOf(Entity entity, in Entity parent) {
            if (entity.WorldId != parent.WorldId) Throw("Child and parent Entity come from a different World");
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            HashSet<int> children = entity.World.EntityInfos[parent.EntityId].Children;
            if (children?.Remove(entity.EntityId) ?? false) {
                entity.World.EntityInfos[entity.EntityId].Parents -= children.Remove;
            }
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveFromParentsOf(Entity parent, in Entity child) => child.RemoveFromChildrenOf(parent);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose(Entity entity) {
            Publisher.Publish(entity.WorldId, new EntityDisposingMessage(entity.EntityId));
            Publisher.Publish(entity.WorldId, new EntityDisposedMessage(entity.EntityId));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Throw(string message) => throw new InvalidOperationException(message);

        /// <inheritdoc />
        public Entity CopyTo(Entity entity, World world)
        {
            if (entity.WorldId == 0) Throw("Entity was not created from a World");

            Entity copy = entity.IsEnabled() ? world.CreateEntity() : world.CreateDisabledEntity();
            try
            {
                Publisher.Publish(entity.WorldId, new EntityCopyMessage(entity.EntityId, copy));

                ref ComponentEnum copyComponents = ref GetComponentsReference(copy);
                copyComponents = GetComponentsReference(entity).Copy();
                if (entity.IsEnabled())
                {
                    Publisher.Publish(entity.WorldId, new EntityEnabledMessage(copy.EntityId, copyComponents));
                }
                else
                {
                    Publisher.Publish(entity.WorldId, new EntityDisabledMessage(copy.EntityId, copyComponents));
                }
            }
            catch
            {
                copy.Dispose();

                throw;
            }

            return copy;
        }
        
        private ref ComponentEnum GetComponentsReference(Entity entity) {
            return ref entity.World.EntityInfos[entity.EntityId].Components;
        }
    }
}