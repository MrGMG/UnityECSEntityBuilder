using System;
using Plugins.ECSEntityBuilder.Variables;
using Unity.Entities;

namespace Plugins.ECSEntityBuilder
{
    public class EntityWrapper
    {
        public Entity Entity { get; private set; }
        public EntityManagerWrapper EntityManagerWrapper { get; }
        public EntityVariableMap Variables { get; }

        public static EntityWrapper Wrap(Entity entity)
        {
            return new EntityWrapper(entity);
        }

        public static EntityWrapper Wrap(Entity entity, EntityManagerWrapper entityManagerWrapper)
        {
            return new EntityWrapper(entity, entityManagerWrapper);
        }

        public EntityWrapper(Entity entity, EntityManagerWrapper entityManagerWrapper, EntityVariableMap variables)
        {
            Entity = entity;
            EntityManagerWrapper = entityManagerWrapper;
            Variables = variables;
        }

        public EntityWrapper(Entity entity, EntityManagerWrapper entityManagerWrapper)
        {
            Entity = entity;
            EntityManagerWrapper = entityManagerWrapper;
            Variables = new EntityVariableMap();
        }

        public EntityWrapper(Entity entity)
        {
            Entity = entity;
            EntityManagerWrapper = EntityManagerWrapper.Default;
            Variables = new EntityVariableMap();
        }

        public EntityWrapper UsingWrapper(EntityManagerWrapper wrapper, Action<EntityWrapper> callback)
        {
            var newEntityWrapper = new EntityWrapper(Entity, wrapper, Variables);
            callback.Invoke(newEntityWrapper);
            return this;
        }

        public EntityWrapper SetVariable<T>(T variable) where T : class, IEntityVariable
        {
            Variables.Set(variable);
            return this;
        }

        public T GetVariable<T>() where T : class, IEntityVariable
        {
            return Variables.Get<T>();
        }

        public EntityWrapper SetName(string name)
        {
            EntityManagerWrapper.SetName(Entity, name);
            return this;
        }

        public EntityWrapper AddComponentData<T>(T component) where T : struct, IComponentData
        {
            EntityManagerWrapper.AddComponentData(Entity, component);
            return this;
        }

        public EntityWrapper SetComponentData<T>(T component) where T : struct, IComponentData
        {
            EntityManagerWrapper.SetComponentData(Entity, component);
            return this;
        }

        public EntityWrapper AddSharedComponentData<T>(T component) where T : struct, ISharedComponentData
        {
            EntityManagerWrapper.AddSharedComponentData(Entity, component);
            return this;
        }

        public DynamicBuffer<T> AddBuffer<T>() where T : struct, IBufferElementData
        {
            return EntityManagerWrapper.AddBuffer<T>(Entity);
        }

        public EntityWrapper AddBuffer<T>(params T[] elements) where T : struct, IBufferElementData
        {
            var buffer = EntityManagerWrapper.AddBuffer<T>(Entity);
            foreach (var element in elements)
                buffer.Add(element);
            return this;
        }

        public EntityWrapper AddElementToBuffer<T>(T element) where T : struct, IBufferElementData
        {
            var buffer = EntityManagerWrapper.AddBuffer<T>(Entity);
            buffer.Add(element);
            return this;
        }

        public DynamicBuffer<T> GetBuffer<T>() where T : struct, IBufferElementData
        {
            return EntityManagerWrapper.GetBuffer<T>(Entity);
        }

        public EntityWrapper Destroy()
        {
            EntityManagerWrapper.Destroy(Entity);
            Entity = Entity.Null;
            return this;
        }
    }
}