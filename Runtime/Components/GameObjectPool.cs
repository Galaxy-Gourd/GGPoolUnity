using GGSharpPool;
using UnityEngine;

namespace GGUnityPool
{
    /// <summary>
    /// Base class for GameObject pools. Used automatically by Pool.cs
    /// </summary>
    public class GameObjectPool : GGSharpPool.Pool
    {
        #region VARIABLES

        internal GameObject PooledGameObject;

        #endregion VARIABLES


        #region CREATION

        protected override IClientPoolable CreateNewPoolable()
        {
            GameObject newObj = Pool.Instantiate(PooledGameObject);
            return newObj.AddComponent<ComponentPooledGameObject>();
        }

        #endregion CREATION
    }
}