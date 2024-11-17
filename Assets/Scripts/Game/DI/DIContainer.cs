using System;
using System.Collections.Generic;

namespace Game.DI
{
    public class DIContainer : IDisposable
    {
        private readonly DIContainer parentContainer;
        private readonly Dictionary<Type, DIEntry> entriesMap = new();
        private readonly HashSet<Type> resolutionsCache = new();

        public DIContainer(DIContainer parentContainer = null)
        {
            this.parentContainer = parentContainer;
        }

        public DIEntry RegisterFactory<T>(Func<DIContainer, T> factory)
        {
            var key = typeof(T);
            
            if (entriesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"DI: Factory with type {key.FullName} has already registered");
            }

            var diEntry = new DIEntry<T>(this, factory);

            entriesMap[key] = diEntry;

            return diEntry;
        }

        public void RegisterInstance<T>(T instance)
        {
            var key = typeof(T);
            
            if (entriesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"DI: Instance with type {key.FullName} has already registered");
            }

            var diEntry = new DIEntry<T>(instance);

            entriesMap[key] = diEntry;
        }

        public bool TryResolve(Type parameterType, out object value)
        {
            value = Resolve(parameterType);

            if (value != null)
            {
                return true;
            }

            return false;
        }

        private object Resolve(Type type)
        {
            if (resolutionsCache.Contains(type))
            {
                throw new Exception($"DI: Cyclic dependency for type {type.FullName}");
            }

            resolutionsCache.Add(type);

            try
            {
                if (entriesMap.TryGetValue(type, out var diEntry))
                {
                    return diEntry.ResolveObject();
                }

                if (parentContainer != null)
                {
                    return parentContainer.Resolve(type);
                }
            }
            finally
            {
                resolutionsCache.Remove(type);
            }

            throw new Exception($"Couldn't find dependency for type {type.FullName}");
        }

        public void Dispose()
        {
            var entries = entriesMap.Values;
            
            foreach (var entry in entries)
            {
                entry.Dispose();
            }
        }
    }
}