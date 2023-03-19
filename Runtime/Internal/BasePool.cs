using System;
using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Pools.Internal {
    public abstract class BasePool : BBMonoBehaviour {

        protected Dictionary<string, int> _poolLookups = new Dictionary<string, int>();
        protected abstract IReadOnlyList<BasePoolDefinition> Definitions{ get; }

        /// <summary>
        /// Awake, creates the definitions. Must be called upon subclassing.
        /// </summary>
        protected void Awake() {
            CreatePoolDefinitions();

            foreach(BasePoolDefinition poolDefinition in Definitions) {
                SetupPoolDefinition(poolDefinition);
            }
        }

        /// <summary>
        /// Sets up a pool definition to be ready for spawning. An error will be logged
        /// if the poolDefinition is invalid
        /// </summary>
        protected void SetupPoolDefinition(BasePoolDefinition poolDefinition) {
            if(poolDefinition.Invalid) {
                Debug.LogException(new Exception("BasePoolDefinition - No prefab to instantiate"), this);
            }

            poolDefinition.SetDefaultParent(CreatePoolContainer(poolDefinition));
            poolDefinition.RefreshInstances();
            _poolLookups.Add(poolDefinition.Name, _poolLookups.Count);
        }

        /// <summary>
        /// Creates a container for the poolDefinition using the name of the pool definition
        /// as its GameObject name
        /// </summary>
        private Transform CreatePoolContainer(BasePoolDefinition poolDefinition) {
            return Utilities.CreateGameObject(poolDefinition.Name, transform).transform;
        }

        /// <summary>
        /// Finds a pool definition for a given name. A type can be provided so that subclasses of 
        /// the pool definitions can be specified.
        /// </summary>
        public T FindPoolDefinition<T>(string definitionName) where T : BasePoolDefinition {
            if(_poolLookups != null && _poolLookups.TryGetValue(definitionName, out int poolId)) {
                return (T)Definitions[poolId];
            }

            return null;
        }

        protected abstract void CreatePoolDefinitions();
        protected virtual void OnSpawn(PoolBehaviour poolBehaviour) { }

        /// <summary>
        /// Returns a pool of a given name. The easiest way to find a pool without 
        /// mapping it directly in the inspector
        /// </summary>
        public static T Find<T>(string name) where T : BasePool {
            foreach(T pool in FindObjectsOfType<T>()) {
                if(string.Equals(pool.name, name)) {
                    return pool;
                }
            }

            Debug.LogError($"Pool.Find - Error finding pool: { name }");

            return null;
        }
    }   
}
