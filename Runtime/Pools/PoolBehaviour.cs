using UnityEngine;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    public class PoolBehaviour : BBMonoBehaviour {

        public delegate void OnCreateEventHandler(PoolBehaviour poolBehaviour);
        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        private ObjectPoolReference _poolReference = null;

        public event OnCreateEventHandler OnCreateEvent;
        public event OnSpawnEventHandler OnSpawnEvent;

        /// <summary>
        /// Called internally upon creation.
        /// </summary>
        internal void _OnCreate(ObjectPoolReference poolReference) {
            _poolReference = poolReference;

            IPoolBehaviour[] callbacks = GetComponents<IPoolBehaviour>();
            foreach(IPoolBehaviour behaviour in callbacks) {
                OnCreateEvent += behaviour.OnCreate;
                OnSpawnEvent += behaviour.OnSpawn;
            }

            Deactivate();
            CallOnCreateCallbacks();
        }

        /// <summary>
        /// Called internally upon Spawn
        /// </summary>
        internal void OnSpawnInternal() {
            Activate();
            CallOnSpawnCallbacks();
        }

        private void CallOnCreateCallbacks() {
            OnCreate();
            OnCreateEvent?.Invoke(this);
        }

        /// <summary>
        /// Calls all of the OnSpawn callbacks in the following order: 
        /// - virtual OnSpawn
        /// - OnSpawnEvent
        /// - poolDefinition.OnSpawnEvent
        /// </summary>
        private void CallOnSpawnCallbacks() {
            OnSpawn();
            OnSpawnEvent?.Invoke(this);
            _poolReference._InvokeOnSpawnEvent(this);
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
