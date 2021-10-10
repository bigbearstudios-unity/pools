using UnityEngine;
using BBUnity;
using BBUnity.Pools.Internal;

namespace BBUnity {

    public class PoolBehaviour : BaseBehaviour {

        public delegate void OnCreateEventHandler(PoolBehaviour poolBehaviour);
        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        /// <summary>
        /// Is the behaviour avalible for spawning, internal variable
        /// </summary>
        private bool _avalible = true;

        /// <summary>
        /// The PoolDefinition which the PoolBehaviour belongs too
        /// </summary>
        private BasePoolDefinition _poolDefinition = null;

        public event OnCreateEventHandler OnCreateEvent;
        public event OnSpawnEventHandler OnSpawnEvent;

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

        /// <summary>
        /// 
        /// </summary>
        public void Destroy() {
            Recycle();
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

        /// <summary>
        /// Can be overriden on the sub class to handle the OnCreate event.
        /// Please note all of the OnCreateEvent Events will still be triggered
        /// even when this method is overriden
        /// </summary>
        public virtual void OnCreate() {}

        /// <summary>
        /// Can be overriden on the sub class to handle the OnSpawn event
        /// Please note all of the OnSpawnEvent Events will still be triggered
        /// even when this method is overriden
        /// </summary>
        public virtual void OnSpawn() {}

        static public void AddOnCreateEvent(MonoBehaviour behaviour, OnCreateEventHandler handler) {
            PoolBehaviour poolBehaviour = behaviour.GetComponent<PoolBehaviour>();
            poolBehaviour.OnCreateEvent += handler;
        }

        static public void AddOnSpawnEvent(MonoBehaviour behaviour, OnSpawnEventHandler handler) {
            PoolBehaviour poolBehaviour = behaviour.GetComponent<PoolBehaviour>();
            poolBehaviour.OnSpawnEvent += handler;
        }
    }
}
