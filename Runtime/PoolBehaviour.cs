using UnityEngine;

using BBUnity;
using BBUnity.Pools;

namespace BBUnity {

    public interface IPoolBehaviour {
        void OnSpawned(PoolBehaviour behaviour);
    }

    public class PoolBehaviour : BaseBehaviour {

        private bool _avalible = true;

        private BasePoolDefinition _poolDefinition = null;
        private BehaviourDelegate<IPoolBehaviour> _delegate;

        internal bool Avalible { get { return _avalible; } }

        private void RegisterDelegate() {
            _delegate = new BehaviourDelegate<IPoolBehaviour>(this);
        }

        private void SetAvalible(bool avalible) {
            _avalible = avalible;
        }

        private void SetPoolDefinition(BasePoolDefinition poolDefinition) {
            _poolDefinition = poolDefinition;
        }

        internal void OnCreate(BasePoolDefinition poolDefinition) {
            SetActive(false);
            SetAvalible(true);
            SetPoolDefinition(poolDefinition);
            RegisterDelegate();
        }

        internal void OnSpawn() {
            SetActive(true);
            SetAvalible(false);

            _delegate.Process(CallOnSpawnDelegate);
        }

        private void CallOnSpawnDelegate(IPoolBehaviour behaviour) {
            behaviour.OnSpawned(this);
        }

        public void Destroy() {
            SetAvalible(true);
            SetActive(false);
        }

        public void Recycle() {
            SetAvalible(true);
            SetActive(false);
        }
    }
}
