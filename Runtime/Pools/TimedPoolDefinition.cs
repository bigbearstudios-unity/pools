using UnityEngine;
using BBUnity.Pools.Internal;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class TimedPoolDefinition : BasePoolDefinition {

        [SerializeField, Tooltip("The time required to spawn an enemy in seconds")]
        private float _spawnTime = 1.0f;
        private float _lastSpawnedAt = 0.0f;

        public float SpawnTime {
            get { return _spawnTime; }
            set { _spawnTime = value; }
        }

        public TimedPoolDefinition(string name, GameObject prefab, float spawnTime, int defaultSize, int maxSize) : base (name, prefab, defaultSize, maxSize) {
            SetSpawnTime(spawnTime);
        }


        public TimedPoolDefinition(GameObject prefab, float spawnTime, int defaultSize, int maxSize) : base (prefab, defaultSize, maxSize) {
            SetSpawnTime(spawnTime);
        }

        public void SetSpawnTime(float spawnTime) {
            _spawnTime = spawnTime;
        }

        internal PoolBehaviour Update(float time) {
            _lastSpawnedAt += time;
            if (_lastSpawnedAt >= _spawnTime) {
                _lastSpawnedAt = _lastSpawnedAt - _spawnTime;
                return Spawn();
            }

            return null;
        }
    }
}