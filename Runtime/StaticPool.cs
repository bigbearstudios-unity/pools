using System.Collections.Generic;

using UnityEngine;

using BBUnity.Pools;

namespace BBUnity {

    [AddComponentMenu("BBUnity/Pools/Static Pool")]
    public class StaticPool : BasePool {

        [Tooltip("The definitions for this pool")]
        [SerializeField]
        private List<StaticPoolDefinition> _poolDefinitions = null;

        protected override IReadOnlyList<BasePoolDefinition> Definitions { get { return _poolDefinitions; } }

        protected override void CreateDefinitions() {
            if(_poolDefinitions == null) {
                _poolDefinitions = new List<StaticPoolDefinition>();
            }
        }

        /// <summary>
        /// Adds a pool definition on the fly. Allows pools to be build using code rather than
        /// from within the Unity editor
        /// </summary>
        /// <param name="poolDefinition"></param>
        /// <returns>True/False for if the PoolDefinition got added</returns>
        public void AddPoolDefinition(StaticPoolDefinition poolDefinition) {
            if(!poolDefinition.Valid) {
                Debug.LogError("Pool.AddPoolDefinition - An invalid definition was passed");
            }

            AddInternalPoolDefinition(poolDefinition);
            SetupPoolDefinition(poolDefinition);
        }

        private void AddInternalPoolDefinition(StaticPoolDefinition poolDefinition) {
            _poolDefinitions.Add(poolDefinition);
        }

        public PoolBehaviour Spawn(string definitionName) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn();
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(position);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position, Quaternion rotation) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(position, rotation);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Vector3 position, Quaternion rotation, Vector3 localScale) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(position, rotation, localScale);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent, Vector3 position) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(parent, position);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent, Vector3 position, Quaternion rotation) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(parent, position, rotation);
            }

            return null;
        }

        public PoolBehaviour Spawn(string definitionName, Transform parent, Vector3 position, Quaternion rotation, Vector3 localScale) {
            StaticPoolDefinition poolDefinition = FindPoolDefinition<StaticPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                return poolDefinition.Spawn(parent, position, rotation, localScale);
            }

            return null;
        }
    }
}


