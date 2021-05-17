using GG.Data.Unity;
using UnityEngine;

namespace GG.Pool.Unity
{
    [CreateAssetMenu(
        fileName = "Data Config Pool",
        menuName = "Configuration Data/Pooling/Gamebject Pool Data")]
    public class DataConfigSOPool : DataConfigSO
    {
        [Header("Prefab")] 
        [Tooltip("The object being pooled.")]
        public GameObject PoolObject;

        [Header("Values")] 
        public int PoolMinimumInstanceLimit = 0;
        public int PoolMaximumInstanceLimit = -1;
        public int SpilloverAllowance = 0;
    }
}