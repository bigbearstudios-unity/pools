using UnityEngine;

using BBUnity.Pools;
using System.Collections.Generic;

namespace BBUnity {

    [AddComponentMenu("BBUnity/Pools/Timed Pool")]
    public class TimedPool : BasePool {

        [Tooltip("The timed definitions for this pool")]
        [SerializeField]
        private List<TimedPoolDefinition> _poolDefinitions = null;

        protected override IReadOnlyList<BasePoolDefinition> Definitions { get { return _poolDefinitions; } }

        protected override void CreateDefinitions() {
            if(_poolDefinitions == null) {
                _poolDefinitions =  new List<TimedPoolDefinition>();
            }
        }

        private void Activate() {
            gameObject.SetActive(true);
        }

        private void Deactivate() {
            gameObject.SetActive(false);
        }

        private void Update() {
            float time = Time.deltaTime;
            foreach (TimedPoolDefinition poolDefinition in _poolDefinitions) {
                poolDefinition.Update(time);
            }
        }

        public void StartSpawning() {
            Activate();

            foreach (TimedPoolDefinition poolDefinition in _poolDefinitions) {
                poolDefinition.Activate();
            }
        }

        public void StartSpawning(string definitionName) {
            Activate();
            TimedPoolDefinition poolDefinition = FindPoolDefinition<TimedPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                poolDefinition.Activate();
            }
        }

        public void StopSpawning() {
            Deactivate();
            
            foreach (TimedPoolDefinition poolDefinition in _poolDefinitions) {
                poolDefinition.Deactivate();
            }
        }

        public void StopSpawning(string definitionName) {
            TimedPoolDefinition poolDefinition = FindPoolDefinition<TimedPoolDefinition>(definitionName);
            if(poolDefinition != null) {
                poolDefinition.Deactivate();
            }
        }

        public void AddPoolDefinition(TimedPoolDefinition poolDefinition) {
            if(!poolDefinition.Valid) {
                Debug.LogError("Pool.AddPoolDefinition - An invalid definition was passed");
            }

            AddInternalPoolDefinition(poolDefinition);
            SetupPoolDefinition(poolDefinition);
        }

        private void AddInternalPoolDefinition(TimedPoolDefinition poolDefinition) {
            _poolDefinitions.Add(poolDefinition);
        }
    }
}

