using System.Collections.Generic;
using GGPoolBase;
using UnityEngine;

namespace GGPoolUnity
{
    /// <summary>
    /// Controls runtime instantiation of pooled GameObjects.
    /// </summary>
    public static class PoolManager
    {
        #region VARIABLES

        /// <summary>
        /// List of our current pools.
        /// </summary>
        private static readonly List<GameObjectPool> _pools = new List<GameObjectPool>();

        #endregion VARIABLES


        #region POOL
        
        public static GameObject Pooled(
            DataConfigSOPool data, 
            Vector3 p, 
            Quaternion r,
            bool updatePoolProperties = false)
        {
            if (updatePoolProperties)
            {
                GetAndSetPoolData(data);
            }
            
            return Pooled(data.PoolObject, p, r);
        }
        
        public static GameObject Pooled(
            GameObject go, 
            Transform t)
        {
            GameObject g = Pooled(go);
            g.transform.SetParent(t);
            g.SetActive(true);
            
            return g;
        }
        
        public static GameObject Pooled(
            DataConfigSOPool data, 
            Transform t,
            bool updatePoolProperties = true)
        {
            if (updatePoolProperties)
            {
                GetAndSetPoolData(data);
            }
            
            return Pooled(data.PoolObject, t);
        }
        
        public static GameObject Pooled(
            GameObject go, 
            Vector3 p, 
            Quaternion r)
        {
            GameObject g = Pooled(go);
            g.transform.SetPositionAndRotation(p, r);
            g.SetActive(true);
            
            return g;
        }
        
        /// <summary>
        /// Returns the next available pooled gameObject.
        /// </summary>
        private static GameObject Pooled(GameObject go)
        {
            // Find the pool for the associated gameObject
            IPool targetPool = GetPoolForObject(go);

            // Return next pooled item
            ComponentPooledGameObject g = targetPool.GetNext() as ComponentPooledGameObject;
            g.OnAnonymousDisable();
            return g.gameObject;
        }
        
        #endregion POOL


        #region INSTANTIATION

        internal static GameObject Instantiate(GameObject go)
        {
            return Object.Instantiate(go).gameObject;
        }

        #endregion INSTANTIATION


        #region UTILITY
        
        public static void SetObjectPoolCapacity(
            GameObject go,
            int capacityMin, 
            int capacityMax)
        {
            IPool targetPool = GetPoolForObject(go, false);
            targetPool.CapacityMin = capacityMin;
            targetPool.CapacityMax = capacityMax;
        }
        
        public static void SetObjectPoolCapacityMin(
            GameObject go,
            int capacityMin)
        {
            IPool targetPool = GetPoolForObject(go, false);
            targetPool.CapacityMin = capacityMin;
        }
        
        public static void SetObjectPoolCapacityMax(
            GameObject go,
            int capacityMax)
        {
            IPool targetPool = GetPoolForObject(go, false);
            targetPool.CapacityMax = capacityMax;
        }
        
        public static void SetObjectPoolSpilloverAllowance(
            GameObject go,
            int spilloverAllowance)
        {
            IPool targetPool = GetPoolForObject(go, false);
            targetPool.SpilloverAllowance = spilloverAllowance;
        }
        
        public static void DeleteGameObjectPool(GameObject go)
        {
            GameObjectPool p = GetPoolForObject(go, false);
            if (p != null)
            {
                ((IPool) p).Clear();
                _pools.Remove(p);
            }
        }

        /// <summary>
        /// Finds and returns the pool for the given object; if none exists, a pool is created
        /// </summary>
        public static GameObjectPool GetPoolForObject(
            GameObject go, 
            bool createIfNotFound = true)
        {
            // Find the pool for the associated gameObject
            GameObjectPool targetPool = null;
            foreach (var pool in _pools)
            {
                if (pool.PooledGameObject == go)
                {
                    targetPool = pool;
                    break;
                }
            }
    
            // If the target is null, there doesn't exist a pool for this GameObject yet
            if (targetPool == null && createIfNotFound)
            {
                targetPool = new GameObjectPool
                {
                    PooledGameObject = go,
                    PoolLabel = "monoPool_" + go.transform.name
                };
                    
                _pools.Add(targetPool);
            }

            return targetPool;
        }
        
        private static void GetAndSetPoolData(DataConfigSOPool data)
        {
            GameObjectPool targetPool = GetPoolForObject(data.PoolObject);
            targetPool.CapacityMin = data.PoolMinimumInstanceLimit;
            targetPool.CapacityMax = data.PoolMaximumInstanceLimit;
            targetPool.SpilloverAllowance = data.SpilloverAllowance;
        }

        #endregion UTILITY
        
        
        #region RESET

        /// <summary>
        /// Resets static values to prevent issues related to domain reloading
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            for (int i = _pools.Count - 1; i >= 0; i--)
            {
                DeleteGameObjectPool(_pools[i].PooledGameObject);
            }
            _pools.Clear();
        }

        #endregion RESET
    }
}
