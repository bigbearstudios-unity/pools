using UnityEngine;
using BBUnity;
using BBUnity.Pools.Internal;

namespace BBUnity {

    public class PoolBehaviour : BaseBehaviour {

        /// <summary>
        /// Is the behaviour avalible for spawning, internal variable
        /// </summary>
        private bool _avalible = true;

        /// <summary>
        /// The PoolDefinition which the PoolBehaviour belongs too
        /// </summary>
        private BasePoolDefinition _poolDefinition = null;

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

            OnCreate();
        }

        internal void _OnSpawn() {
            SetActive(true);
            SetAvalible(false);

            OnSpawn();
        }

        public void Destroy() {
            SetAvalible(true);
            SetActive(false);
        }

        public void Recycle() {
            SetAvalible(true);
            SetActive(false);
        }

        public virtual void OnCreate() {

        }

        public virtual void OnSpawn() {

        }
    }
}
