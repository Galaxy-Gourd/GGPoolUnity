using GGPoolBase;
using UnityEngine;

namespace GGPoolUnity
{
    /// <summary>
    /// Base class for GameObject pools. Used automatically by Pool.cs
    /// </summary>
    public class GameObjectPool : GGPoolBase.Pool
    {
        #region VARIABLES

        internal GameObject PooledGameObject;

        #endregion VARIABLES


        #region CREATION

        protected override IClientPoolable CreateNewPoolable()
        {
            GameObject newObj = PoolManager.Instantiate(PooledGameObject);
            return newObj.AddComponent<ComponentPooledGameObject>();
        }

        #endregion CREATION
    }
}