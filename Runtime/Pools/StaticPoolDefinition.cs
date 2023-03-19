using UnityEngine;
using BBUnity.Pools.Internal;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class StaticPoolDefinition : BasePoolDefinition {

        public StaticPoolDefinition() : base() {}
        public StaticPoolDefinition(GameObject prefab) : base(prefab) {}
        public StaticPoolDefinition(string name, GameObject prefab) : base(name, prefab) {}
        public StaticPoolDefinition(GameObject prefab, int defaultSize) : base(prefab, defaultSize) {}
        public StaticPoolDefinition(string name, GameObject prefab, int defaultSize) : base(name, prefab, defaultSize) {}
        public StaticPoolDefinition(GameObject prefab, int defaultSize, int maximumSize) : base(prefab, defaultSize, maximumSize) { }
        public StaticPoolDefinition(string name, GameObject prefab, int defaultSize, int maximumSize) : base(name, prefab, defaultSize, maximumSize) {}

        public PoolBehaviour Spawn(string definitionName, Vector3 position) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                poolBehaviour.transform.position = position;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position, Vector3 scale) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                poolBehaviour.transform.position = position;
                poolBehaviour.transform.localScale = scale;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                poolBehaviour.SetParent(parent);
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent, Vector3 position, Vector3 scale) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                poolBehaviour.SetParent(parent);
                poolBehaviour.transform.position = position;
                poolBehaviour.transform.localScale = scale;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, System.Action<PoolBehaviour> beforeSpawn) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                beforeSpawn(poolBehaviour);
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, System.Action<PoolBehaviour> beforeSpawn, System.Action<PoolBehaviour> afterSpawn) {
            PoolBehaviour poolBehaviour = _Spawn();
            if(poolBehaviour != null) {
                beforeSpawn(poolBehaviour);
                poolBehaviour._OnSpawn();
                afterSpawn(poolBehaviour);
            }

            return null;
        }
    }
}
