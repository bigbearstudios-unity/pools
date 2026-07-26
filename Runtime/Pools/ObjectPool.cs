using UnityEngine;

using System;
using System.Collections.Generic;

namespace BBUnity.Pools {

    /// <summary>
    /// A simple static pool which can contain multiple spawnable definitions
    /// </summary>
    [AddComponentMenu("BBUnity/Pools/Object Pool")]
    public class ObjectPool : MonoBehaviour {

        [Tooltip("The definitions for this pool")]
        [SerializeField]
        private List<ObjectPoolReference> _poolReferences;

        // Keyed directly by ObjectPoolReference rather than a list index, so there's no
        // insertion-order invariant to keep in sync if reference removal is ever added.
        protected Dictionary<string, ObjectPoolReference> _poolReferenceLookups = new Dictionary<string, ObjectPoolReference>();

        /*
         * MonoBehaviour Methods
         */

        protected void Awake() {
            _poolReferences ??= new List<ObjectPoolReference>();

            foreach(ObjectPoolReference poolReference in _poolReferences) {
                SetupPoolDefinition(poolReference);
            }
        }

        private void OnValidate() {
            if (_poolReferences == null) { return; }

            foreach (ObjectPoolReference poolReference in _poolReferences) {
                // Re-applies the same non-positive-MaximumSize warning as SetMaximumSize(),
                // which inspector edits bypass entirely since they write the serialized field
                // directly rather than calling the setter method.
                poolReference.SetMaximumSize(poolReference.MaximumSize);
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

            if(_poolReferenceLookups.ContainsKey(poolDefinition.Name)) {
                throw new Exception($"ObjectPool on '{gameObject.name}' already has a pool reference named '{poolDefinition.Name}'.");
            }

            poolDefinition.InitialiseInstances();
            _poolReferenceLookups.Add(poolDefinition.Name, poolDefinition);
        }

        public ObjectPoolReference FindPoolReference(string definitionName, bool raiseError = false) {
            if(!_poolReferenceLookups.TryGetValue(definitionName, out ObjectPoolReference poolReference)) {
                if(raiseError) {
                    throw new Exception($"No Pool Definition found for: {definitionName}");
                }

                return null;
            }

            return poolReference;
        }

        /// <summary>
        /// Spawns an instance from the named pool reference. Returns null (logging an error) if
        /// no reference with that name is registered, or if the reference is at MaximumSize and
        /// has no inactive instance available to reuse — callers must null-check the result.
        /// </summary>
        public PoolBehaviour Spawn(string definitionName) {
            ObjectPoolReference poolDefinition = FindPoolReference(definitionName);
            if (poolDefinition == null) {
                Debug.LogError($"ObjectPool on '{gameObject.name}' has no pool reference named '{definitionName}'.", this);
                return null;
            }

            return poolDefinition.Spawn();
        }

        public void AddPoolReference(ObjectPoolReference poolReference) {
            if(poolReference.Invalid) {
                throw new Exception("Pool.AddPoolDefinition - An invalid definition was passed");
            }

            _poolReferences.Add(poolReference);
            SetupPoolDefinition(poolReference);
        }

        /// <summary>
        /// Finds an ObjectPool in the scene by GameObject name. Returns null (rather than
        /// throwing) if none is found, so callers can fall back to creating one.
        /// </summary>
        public static ObjectPool FindInScene(string name) {
            foreach(ObjectPool pool in FindObjectsByType<ObjectPool>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
                if(string.Equals(pool.name, name)) { return pool; }
            }

            return null;
        }
    }
}


