using UnityEngine;

namespace BBUnity.Pools {

    /// <summary>
    ///
    /// </summary>
    public class PoolBehaviour : MonoBehaviour {

        public delegate void OnCreateEventHandler(PoolBehaviour poolBehaviour);
        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);
        public delegate void OnReturnEventHandler(PoolBehaviour poolBehaviour);

        private ObjectPoolReference _poolReference = null;

        // Prefer IPoolBehaviour for logic that lives on the pooled prefab itself (see
        // IPoolBehaviour for why). These events are for external code that calls
        // ObjectPool.Spawn()/PoolBehaviour.ReturnToPool() without owning the prefab.
        public event OnCreateEventHandler OnCreateEvent;
        public event OnSpawnEventHandler OnSpawnEvent;
        public event OnReturnEventHandler OnReturnEvent;

        /// <summary>
        /// Called internally upon creation.
        /// </summary>
        internal void _OnCreate(ObjectPoolReference poolReference) {
            _poolReference = poolReference;

            IPoolBehaviour[] callbacks = GetComponents<IPoolBehaviour>();
            foreach(IPoolBehaviour behaviour in callbacks) {
                OnCreateEvent += behaviour.OnCreate;
                OnSpawnEvent += behaviour.OnSpawn;
                OnReturnEvent += behaviour.OnReturn;
            }

            gameObject.SetActive(false);
            CallOnCreateCallbacks();
        }

        /// <summary>
        /// Called internally upon Spawn
        /// </summary>
        internal void OnSpawnInternal() {
            gameObject.SetActive(true);
            CallOnSpawnCallbacks();
        }

        /// <summary>
        /// Returns this instance to its pool, ready to be reused by a future Spawn() call.
        /// </summary>
        public void ReturnToPool() {
            CallOnReturnCallbacks();
            gameObject.SetActive(false);
        }

        private void OnDestroy() {
            _poolReference?.RemoveInstance(this);
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
        /// Calls all of the OnReturn callbacks in the following order:
        /// - virtual OnReturn
        /// - OnReturnEvent
        /// </summary>
        private void CallOnReturnCallbacks() {
            OnReturn();
            OnReturnEvent?.Invoke(this);
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

        /// <summary>
        /// Can be overriden on the sub class to handle the OnReturn event (see ReturnToPool()).
        /// Please note all of the OnReturnEvent Events will still be triggered
        /// even when this method is overriden
        /// </summary>
        public virtual void OnReturn() {}

        static public void AddOnCreateEvent(MonoBehaviour behaviour, OnCreateEventHandler handler) {
            PoolBehaviour poolBehaviour = behaviour.GetComponent<PoolBehaviour>();
            if(poolBehaviour == null) {
                throw new MissingComponentException($"AddOnCreateEvent requires a PoolBehaviour component on '{behaviour.gameObject.name}'.");
            }

            poolBehaviour.OnCreateEvent += handler;
        }

        static public void AddOnSpawnEvent(MonoBehaviour behaviour, OnSpawnEventHandler handler) {
            PoolBehaviour poolBehaviour = behaviour.GetComponent<PoolBehaviour>();
            if(poolBehaviour == null) {
                throw new MissingComponentException($"AddOnSpawnEvent requires a PoolBehaviour component on '{behaviour.gameObject.name}'.");
            }

            poolBehaviour.OnSpawnEvent += handler;
        }
    }
}
