using UnityEngine;
using BBUnity.Pools.Internal;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class TimedPoolDefinition : BasePoolDefinition {

        public const bool DefaultActive = true;
        public const float DefaultSpawnTime = 1.0f;

        [SerializeField]
        private bool _active = DefaultActive;

        [SerializeField, Tooltip("The time required to spawn an enemy in seconds")]
        private float _spawnTime = DefaultSpawnTime;

        private float _lastSpawnedAt = 0.0f;

        public bool Active { get { return _active; } }
        private bool ShouldSpawn { get { return _lastSpawnedAt >= _spawnTime; } }

        public float SpawnTime {
            get { return _spawnTime; }
            set { _spawnTime = value; }
        }

        public TimedPoolDefinition(GameObject prefab, float spawnTime) : base(prefab) {
           SetSpawnTime(spawnTime);
        }

        public TimedPoolDefinition(GameObject prefab, float spawnTime, int defaultSize) : base(prefab, defaultSize) {
            SetSpawnTime(spawnTime);
        }

        public TimedPoolDefinition(GameObject prefab, float spawnTime, int defaultSize, int maxSize) : base (prefab, defaultSize, maxSize) {
            SetSpawnTime(spawnTime);
        }

        public TimedPoolDefinition(GameObject prefab, bool active, float spawnTime) : base(prefab) {
            SetSpawnTime(spawnTime);
            SetActive(active);
        }

        public TimedPoolDefinition(GameObject prefab, bool active, float spawnTime, int defaultSize) : base(prefab, defaultSize) {
            SetSpawnTime(spawnTime);
            SetActive(active);
        }

        public TimedPoolDefinition(GameObject prefab, bool active, float spawnTime, int defaultSize, int maxSize) : base (prefab, defaultSize, maxSize) {
            SetSpawnTime(spawnTime);
            SetActive(active);
        }

        public void SetSpawnTime(float spawnTime) {
            _spawnTime = spawnTime;
        }

        public void SetActive(bool active) {
            _active = active;
        }

        public void Activate() {
            _active = true;
        }

        public void Deactivate() {
            _active = false;
        }

        internal PoolBehaviour Update(float time) {
            if(Active) {
                _lastSpawnedAt += time;
                if (ShouldSpawn) {
                    _lastSpawnedAt = _lastSpawnedAt - _spawnTime;
                    return Spawn();
                }
            }

            return null;
        }
    }
}