using UnityEngine;
using BBUnity;
using BBUnity.Pools.Internal;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    public class PoolBehaviour : BBMonoBehaviour {

        public delegate void OnCreateEventHandler(PoolBehaviour poolBehaviour);
        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        private bool _avalible = true;
        private BasePoolDefinition _poolDefinition = null;

        public event OnCreateEventHandler OnCreateEvent;
        public event OnSpawnEventHandler OnSpawnEvent;

        public bool Avalible { get { return _avalible; } }

        private void SetAvalible(bool avalible) {
            _avalible = avalible;
        }

        private void SetPoolDefinition(BasePoolDefinition poolDefinition) {
            _poolDefinition = poolDefinition;
        }

        /// <summary>
        /// Iterates through all of the attached components to find IPoolBehaviour,
        /// foreach one found assigns an onCreateEvent, onSpawnEvent
        /// </summary>
        private void AssignCallbackInterfaceEvents() {
            IPoolBehaviour[] callbacks = GetComponents<IPoolBehaviour>();
            foreach(IPoolBehaviour behaviour in callbacks) {
                OnCreateEvent += behaviour.OnCreate;
                OnSpawnEvent += behaviour.OnSpawn;
            }
            
        }

        /// <summary>
        /// Called internally upon creation.
        /// </summary>
        internal void _OnCreate(BasePoolDefinition poolDefinition) {
            DeactivateGameObject();
            SetAvalible(true);
            SetPoolDefinition(poolDefinition);

            AssignCallbackInterfaceEvents();

            CallOnCreateCallbacks();
        }

        /// <summary>
        /// Called internally upon Spawn
        /// </summary>
        internal void _OnSpawn() {
            ActivateGameObject();
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
            DeactivateGameObject();
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
            _poolDefinition._InvokeOnSpawnEvent(this);
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
