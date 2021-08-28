using UnityEngine;
using BBUnity;
using BBUnity.Pools.Internal;

namespace BBUnity {

    public class PoolBehaviour : BaseBehaviour {

        public delegate void OnCreateHandler(PoolBehaviour poolBehaviour);
        public delegate void OnSpawnHandler(PoolBehaviour poolBehaviour);

        /// <summary>
        /// Is the behaviour avalible for spawning, internal variable
        /// </summary>
        private bool _avalible = true;

        /// <summary>
        /// The PoolDefinition which the PoolBehaviour belongs too
        /// </summary>
        private BasePoolDefinition _poolDefinition = null;

        public event OnCreateHandler OnCreateEvent;
        public event OnSpawnHandler OnSpawnEvent;

        internal bool Avalible { get { return _avalible; } }

        private void SetAvalible(bool avalible) {
            _avalible = avalible;
        }

        private void SetPoolDefinition(BasePoolDefinition poolDefinition) {
            _poolDefinition = poolDefinition;
        }

        internal void _OnCreate(BasePoolDefinition poolDefinition) {
            SetActive(false);
            SetAvalible(true);
            SetPoolDefinition(poolDefinition);

            CallOnCreateCallbacks();
        }

        internal void _OnSpawn() {
            SetActive(true);
            SetAvalible(false);

            CallOnSpawnCallbacks();
        }

        public void Destroy() {
            SetAvalible(true);
            SetActive(false);
        }

        public void Recycle() {
            SetAvalible(true);
            SetActive(false);
        }

        private void CallOnCreateCallbacks() {
            OnCreate();

            OnCreateEvent?.Invoke(this);
        }

        private void CallOnSpawnCallbacks() {
            OnSpawn();

            OnSpawnEvent?.Invoke(this);
        }

        public virtual void OnCreate() {

        }

        public virtual void OnSpawn() {

        }
    }
}
