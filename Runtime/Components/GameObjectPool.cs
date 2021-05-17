using GG.Pool.Base;
using UnityEngine;

namespace GG.Pool.Unity
{
    /// <summary>
    /// Base class for GameObject pools. Used automatically by Pool.cs
    /// </summary>
    public class GameObjectPool : Base.Pool
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