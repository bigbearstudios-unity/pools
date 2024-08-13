using UnityEngine;

using System;
using System.Collections.Generic;

namespace BBUnity.Pools {

    /// <summary>
    /// A simple static pool which can contain multiple spawnable definitions
    /// </summary>
    [AddComponentMenu("BBUnity/Pools/Object Pool")]
    public class ObjectPool : BBMonoBehaviour {

        [Tooltip("The definitions for this pool")]
        [SerializeField]
        private List<ObjectPoolReference> _poolReferences;
        protected Dictionary<string, int> _poolReferenceLookups = new Dictionary<string, int>();

        /*
         * MonoBehaviour Methods
         */

        protected void Awake() {
            _poolReferences ??= new List<ObjectPoolReference>();

            foreach(ObjectPoolReference poolReference in _poolReferences) {
                SetupPoolDefinition(poolReference);
            }
        }

        /// <summary>
        /// Sets up a pool definition to be ready for spawning. An error will be logged
        /// if the poolDefinition is invalid
        /// </summary>
        protected void SetupPoolDefinition(ObjectPoolReference poolDefinition) {
            if(poolDefinition.Invalid) {
                throw new Exception("BasePoolDefinition - No prefab to instantiate");
            }

            poolDefinition.RefreshInstances();
            _poolReferenceLookups.Add(poolDefinition.Name, _poolReferenceLookups.Count);
        }

        public ObjectPoolReference FindPoolDefinition(string definitionName) {
            if(!_poolReferenceLookups.TryGetValue(definitionName, out int poolId)) {
                throw new Exception($"No Pool Definition found for: {definitionName}");
            }

            return _poolReferences[poolId];
        }

        public PoolBehaviour Spawn(string definitionName) {
            ObjectPoolReference poolDefinition = FindPoolDefinition(definitionName);
            return poolDefinition.Spawn();
        }

        public void AddPoolDefinition(ObjectPoolReference poolReference) {
            if(poolReference.Invalid) {
                Debug.LogError("Pool.AddPoolDefinition - An invalid definition was passed");
            }

            _poolReferences.Add(poolReference);
            SetupPoolDefinition(poolReference);
        }

        public static ObjectPool FindInScene(string name) {
            foreach(ObjectPool pool in FindObjectsOfType<ObjectPool>()) {
                if(string.Equals(pool.name, name)) { return pool; }
            }

            Debug.LogError($"Pool.Find - Error finding pool: { name }");

            return null;
        }
    }
}


