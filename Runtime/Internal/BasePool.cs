using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Pools.Internal {
    public abstract class BasePool : BaseBehaviour {

        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        /// <summary>
        /// The Dictionary which holds all of the poolLookups. This is a collection
        /// of strings with their associated int
        /// </summary>
        protected Dictionary<string, int> _poolLookups;

        /// <summary>
        /// The OnSpawnEvent, calls the delegates when
        /// </summary>
        public event OnSpawnEventHandler OnSpawnEvent;

        protected abstract IReadOnlyList<BasePoolDefinition> Definitions{ get; }

        private void Awake() {
            CreateDefinitions();
        }

        private void Start() {
            SetupDefinitions();
        }

        protected virtual void CreateDefinitions() {}

        /// <summary>
        /// CreateDefinitions. Only used when the poolLookups have been assigned
        /// via the Unity editor
        /// </summary>
        private void SetupDefinitions() {
            IEnumerable<BasePoolDefinition> poolDefinitions = Definitions;
            foreach(BasePoolDefinition poolDefinition in poolDefinitions) {
                if(poolDefinition.Valid) {
                    SetupPoolDefinition(poolDefinition);
                }
            }
        }

        private void AddPoolLookup(string poolDefinitionName) {
            if(_poolLookups == null) {
                _poolLookups = new Dictionary<string, int>();
            }

            _poolLookups.Add(poolDefinitionName, _poolLookups.Count);
        }

        protected void SetupPoolDefinition(BasePoolDefinition poolDefinition) {
            poolDefinition.SetDefaultParent(CreatePoolContainer(poolDefinition));
            poolDefinition.RefreshInstances();

            AddPoolLookup(poolDefinition.Name);
        }

        private Transform CreatePoolContainer(BasePoolDefinition poolDefinition) {
            GameObject obj = Utilities.CreateGameObject(poolDefinition.Name, transform);
            obj.transform.position = Vector3.zero;
            obj.transform.position = Vector3.zero;

            return obj.transform;
        }

        public T FindPoolDefinition<T>(string definitionName) where T : BasePoolDefinition {
            if(_poolLookups != null) {
                if(_poolLookups.TryGetValue(definitionName, out int poolId)) {
                    return (T)Definitions[poolId];
                }
            }

            return null;
        }

        protected void _OnSpawn(PoolBehaviour poolBehaviour) {
            OnSpawn(poolBehaviour);
            OnSpawnEvent?.Invoke(poolBehaviour);
        }

        protected virtual void OnSpawn(PoolBehaviour poolBehaviour) {
            
        }

        /*
         * Static Methods
         */

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
