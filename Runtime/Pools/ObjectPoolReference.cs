using UnityEngine;
using System.Collections.Generic;

using BBUnity;

namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ObjectPoolReference {
        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        [SerializeField, Tooltip("The name of the Definition. This is used to access the definition, defaults to the prefab name")]
        private string _name = null;

        [SerializeField, Tooltip("The prefab which will be instantiated")]
        private GameObject _prefab = null;

        [SerializeField, Tooltip("The starting size of the pool. This will be filled with instantiated prefabs")]
        private int _startingSize = 0;

        [SerializeField, Tooltip("The maximum size of the pool.")]
        private int _maximumSize = 10;

        private List<PoolBehaviour> _instances = null;
        public event OnSpawnEventHandler OnSpawnEvent;

        public string Name {
            get { return (_name != null && _name.Length > 0) ? _name : _prefab.name; }
            set { _name = value; }
        }

        public GameObject Prefab {
            get { return _prefab; }
            set { _prefab = value; }
        }

        public bool HasPrefab {
            get { return _prefab != null; }
        }

        public int StartingSize {
            get { return _startingSize; }
            set { _startingSize = value; }
        }

        public int MaximumSize {
            get { return _maximumSize; }
            set { _maximumSize = value; }
        }

        public bool AllowGrowth {
            get { return NumberOfInstances < _maximumSize; }
        }

        public int NumberOfInstances {
            get { return _instances != null ? _instances.Count : 0; }
        }

        public bool Valid {
            get { return _prefab != null; }
        }

        public bool Invalid {
            get { return !Valid; }
        }

        public ObjectPoolReference(GameObject prefab, int startingSize, int maximumSize) {
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            SetMaximumSize(maximumSize);
            RefreshInstances();
        }

        public ObjectPoolReference(string name, GameObject prefab, int startingSize, int maximumSize) {
            SetName(name);
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            SetMaximumSize(maximumSize);
            RefreshInstances();
        }

        public void SetName(string name) {
            _name = name;
        }

        public void SetPrefab(GameObject prefab) {
            _prefab = prefab;
        }

        public void SetStartingSize(int size) {
            _startingSize = size;
        }

        public void SetMaximumSize(int size) {
            _maximumSize = size;
        }

        private PoolBehaviour CreateInstance() {
            PoolBehaviour poolBehaviour = Utilities.Create.InstantiatedGameObjectWithComponent<PoolBehaviour>(_prefab);

            poolBehaviour._OnCreate(this);

            _instances.Add(poolBehaviour);

            return poolBehaviour;
        }

        public void RefreshInstances() {
            _instances = new List<PoolBehaviour>(_startingSize);
            while(_instances.Count < _startingSize) {
                CreateInstance();
            }
        }

        internal PoolBehaviour GetOrCreateInstance() {
            foreach(PoolBehaviour instance in _instances) {
                if(!instance.Active) { return instance; }
            }
            
            if(AllowGrowth) {
                return CreateInstance();
            }

            return null;
        }

        internal void _InvokeOnSpawnEvent(PoolBehaviour poolBehaviour) {
            OnSpawnEvent?.Invoke(poolBehaviour);
        }

        /*
         * Public Spawn Methods
         */

        /*
         * Spawns the PoolBehaviour, will call the underlying OnSpawnEvent if set and also
         * poolBehaviour.OnSpawn
         */
        public PoolBehaviour Spawn() {
            PoolBehaviour poolBehaviour = GetOrCreateInstance();
            if(poolBehaviour != null) {
                poolBehaviour.OnSpawnInternal();
            }

            return poolBehaviour;
        }
    }
}
