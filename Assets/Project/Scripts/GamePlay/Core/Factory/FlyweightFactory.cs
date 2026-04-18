using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Project.Scripts.GamePlay.Core.Factory
{
    public abstract class FlyweightFactory<T, TType> where T : Flyweight<TType> where TType : Enum
    {
        protected virtual bool CollectionCheck => true;
        protected virtual int DefaultCapacity => 10;
        protected virtual int MaxPoolSize => 100;
        private Dictionary<TType, IObjectPool<T>> _pools = new();

        protected T Spawn(TType type) => GetPoolFor(type)?.Get();
        protected void ReturnToPool(T f) => GetPoolFor(f.Type)?.Release(f);


        protected abstract T Create(TType type);

        protected virtual void OnGet(T flyweight)
        {
            flyweight.gameObject.SetActive(true);
        }

        protected virtual void OnRelease(T flyweight)
        {
            flyweight.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyPoolObject(T flyweight)
        {
         
                Object.Destroy(flyweight.gameObject);
            
        }

        public void ClearPools()
        {
            _pools = new();
        }

        protected IObjectPool<T> GetPoolFor(TType type)
        {
            IObjectPool<T> pool;

            if (_pools.TryGetValue(type, out pool)) return pool;

            pool = new ObjectPool<T>(
                () => Create(type),
                OnGet,
                OnRelease,
                OnDestroyPoolObject,
                CollectionCheck,
                DefaultCapacity,
                MaxPoolSize
            );
            _pools.Add(type, pool);
            return pool;
        }
    }
}