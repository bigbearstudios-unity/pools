using UnityEngine;
using BBUnity.Pools.Internal;
using System.Collections.Generic;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    [AddComponentMenu("BBUnity/Pools/Static Pool")]
    public class StaticPool : BasePool {

        [Tooltip("The definitions for this pool")]
        [SerializeField]
        private List<StaticPoolDefinition> _poolDefinitions = null;

        protected override IReadOnlyList<BasePoolDefinition> Definitions { get { return _poolDefinitions; } }

        protected override void CreatePoolDefinitions() {
            if(_poolDefinitions is null) {
                _poolDefinitions = new List<StaticPoolDefinition>();
            }
        }

        public StaticPoolDefinition FindPoolDefinition(string definitionName) {
            if(_poolLookups != null && _poolLookups.TryGetValue(definitionName, out int poolId)) {
                return (StaticPoolDefinition)Definitions[poolId];
            }

            return null;
        }

        private PoolBehaviour _Spawn(string definitionName) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition(definitionName);
            if(poolDefinition != null) {
                return poolDefinition._Spawn();
            } else {
                Debug.LogErrorFormat("No Pool Definition found for the name: {0}", definitionName);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                poolBehaviour.transform.position = position;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position, Vector3 scale) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                poolBehaviour.transform.position = position;
                poolBehaviour.transform.localScale = scale;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                poolBehaviour.SetParent(parent);
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent, Vector3 position, Vector3 scale) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                poolBehaviour.SetParent(parent);
                poolBehaviour.transform.position = position;
                poolBehaviour.transform.localScale = scale;
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, System.Action<PoolBehaviour> beforeSpawn) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                beforeSpawn(poolBehaviour);
                poolBehaviour._OnSpawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, System.Action<PoolBehaviour> beforeSpawn, System.Action<PoolBehaviour> afterSpawn) {
            PoolBehaviour poolBehaviour = _Spawn(definitionName);
            if(poolBehaviour != null) {
                beforeSpawn(poolBehaviour);
                poolBehaviour._OnSpawn();
                afterSpawn(poolBehaviour);
            }

            return null;
        }

        public void AddPoolDefinition(StaticPoolDefinition poolDefinition) {
            if(poolDefinition.Invalid) {
                Debug.LogError("Pool.AddPoolDefinition - An invalid definition was passed");
            }

            AddInternalPoolDefinition(poolDefinition);
            SetupPoolDefinition(poolDefinition);
        }

        private void AddInternalPoolDefinition(StaticPoolDefinition poolDefinition) {
            _poolDefinitions.Add(poolDefinition);
        }

        /// <summary>
        /// Returns a pool of a given name. The easiest way to find a pool without 
        /// mapping it directly in the inspector
        /// </summary>
        public static StaticPool Find(string name) {
            foreach(StaticPool pool in FindObjectsOfType<StaticPool>()) {
                if(string.Equals(pool.name, name)) {
                    return pool;
                }
            }

            Debug.LogError($"Pool.Find - Error finding pool: { name }");

            return null;
        }
    }
}


