using System;
using System.Collections.Generic;

namespace Project.Scripts.Common.IdProvider
{
    public static class IdProvider
    {
        private static readonly Dictionary<Type, List<int>> IdsByType = new();

        public static int GetNext<T>()
        {
            return GetNext(typeof(T));
        }

        public static int GetNext(Type type)
        {
            if (!IdsByType.TryGetValue(type, out var collection))
            {
                collection = new List<int>();
                IdsByType[type] = collection;
            }
            var id = collection.Count;
            collection.Add(id);
            return id;
        }

        public static IReadOnlyList<int> CreateIdCollection<T>(int count)
        {
            var type = typeof(T);
            if (!IdsByType.TryGetValue(type, out var collection))
            {
                collection = new List<int>();
                IdsByType[type] = collection;
            }
            var list = new List<int>(count);
            var next = collection.Count;
            for (var i = 0; i < count; i++)
            {
                list.Add(next);
                collection.Add(next);
                next++;
            }
            return list;
        }

        public enum ConstId
        {
            Player=-1,
            None=-2
        }
    }
}
