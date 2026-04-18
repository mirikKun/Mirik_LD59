using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Entity
{
    public class ComponentsRegistry
    {
        private Dictionary<Type, Component> _componentMap;

        public ComponentsRegistry(List<Component> components)
        {
            _componentMap = new Dictionary<Type, Component>();
            foreach (Component component in components)
            {
                _componentMap.TryAdd(component.GetType(), component);
            }
        }

        public ComponentsRegistry(List<Component> components, ActorEntity actorEntity)
        {
            _componentMap = new Dictionary<Type, Component>();
            foreach (Component component in components)
            {
                _componentMap.TryAdd(component.GetType(), component);
            }

            foreach (Component component in components)
            {
                if (component is EntityComponent entityComponent)
                {
                    entityComponent.InitEntity(actorEntity);
                }
            }
        }

        public void StartEntities()
        {
            List<Component> components = _componentMap.Values.ToList();
            foreach (Component component in components)
            {
                if (component is EntityComponent entityComponent)
                {
                    entityComponent.StartEntity();
                }
            }
        }
        public bool Has<T>() where T : class
        {
            return _componentMap.ContainsKey(typeof(T));
        }
        public T Get<T>() where T : class
        {
            if (_componentMap.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }
            else if (TryGetComponentByInterface<T>(out var newComponent))
            {
                _componentMap.TryAdd(newComponent.GetType(), newComponent);

                return newComponent as T;
            }
            else
            {
                throw new KeyNotFoundException($"Component of type {typeof(T)} not found in registry.");
            }
        }

        public bool TryGet<T>(out T value) where T : class
        {
            if (_componentMap.TryGetValue(typeof(T), out var component))
            {
                value = component as T;
                return true;
            }
            else if (TryGetComponentByInterface<T>(out var newComponent))
            {
                _componentMap.TryAdd(newComponent.GetType(), newComponent);
                value = newComponent as T;
                return true;
            }

            value = null;
            return false;
        }

        public T GetComponent<T>() where T : Component
        {
            if (_componentMap.TryGetValue(typeof(T), out Component component))
            {
                return (T)component;
            }

            throw new KeyNotFoundException($"Component of type {typeof(T)} not found in registry.");
        }

        public bool TryGetComponentByInterface<T>(out Component value) where T : class
        {
            foreach (var component in _componentMap.Values)
            {
                if (component is T)
                {
                    value = component;
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}