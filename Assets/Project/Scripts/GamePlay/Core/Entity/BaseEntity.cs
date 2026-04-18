using System.Collections.Generic;
using Project.Scripts.Common.IdProvider;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Entity
{
    public abstract class BaseEntity:MonoBehaviour
    {
        [SerializeField] protected List<Component> _componentsList;
        public int Id { get; protected set; }
        public ComponentsRegistry Components { get; protected set; }
        public T Get<T>() where T : class
        {
            return Components.Get<T>();
        }

        public bool Has<T>() where T : class
        {
            return Components.Has<T>();
        }
        public bool TryGet<T>(out T component) where T : class
        {
            return Components.TryGet(out component);
        }

        protected virtual void Awake()
        {
            Id = GetNextId();
            InitComponentsRegistry();
        }

        protected virtual int GetNextId() => IdProvider.GetNext<BaseEntity>();

        protected virtual void InitComponentsRegistry()
        {
            Components= new ComponentsRegistry(_componentsList);
        }
        [ContextMenu("Get Components")]
        private void GetComponents()
        {
            _componentsList = new List<Component>(GetComponentsInChildren<EntityComponent>());
        }
    }
}