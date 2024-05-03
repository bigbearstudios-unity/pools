using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Pools.Internal {

    [System.Serializable]
    public class BasePoolDefinition {

        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        [SerializeField, Tooltip("The name of the Definition. This is used to access the definition, defaults to the prefab name")]
        private string _name = null;

        [SerializeField, Tooltip("The prefab which will be instanciated")]
        private GameObject _prefab = null;

        [SerializeField, Tooltip("The starting size of the pool. This will be filled with instanciated prefabs")]
        private int _startingSize = 0;

        [SerializeField, Tooltip("The maximum size of the pool.")]
        private int _maximumSize = 10;

        [SerializeField, Tooltip("Set transform to parent")]
        private bool _spawnOnParent = true;

        [SerializeField, Tooltip("The parent of the spawned PoolBehaviour. Default: null, will spawn at the scene root")]
        private Transform _defaultParent = null;

        [SerializeField, Tooltip("The spawn point of the spawn PoolBehaviour. Default: null, will spawn at the centre of its parent")]
        private Transform _spawnPoint = null;

        // TODO We should probably allow this functionality in another way?
        // [Tooltip("Should we use disabled instances as well as 'avalible' instances")]
        // [SerializeField]
        // private bool _useDisabledInstances = true;

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

        public int NumberOfAvalibleInstances {
            get {
                int avalibleInstances = 0;
                foreach(PoolBehaviour behaviour in _instances) {
                    if(behaviour.Avalible) {
                        avalibleInstances++;
                    }
                }

                return avalibleInstances;
            }
        }

        public bool Valid {
            get { return (_prefab != null); }
        }

        public bool Invalid {
            get { return !Valid; }
        }

        public BasePoolDefinition(GameObject prefab, int startingSize, int maximumSize) {
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            SetMaximumSize(maximumSize);
            RefreshInstances();
        }

        public BasePoolDefinition(string name, GameObject prefab, int startingSize, int maximumSize) {
            SetName(name);
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            SetMaximumSize(maximumSize);
            RefreshInstances();
        }

        public void SetName(string name) {
            _name = name;
        }

        public void SetDefaultParent(Transform defaultParent) {
            _defaultParent = defaultParent;
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
            PoolBehaviour poolBehaviour = Utilities.InstantiateWithComponent<PoolBehaviour>(_prefab, _defaultParent, true);

            poolBehaviour._OnCreate(this);

            _instances.Add(poolBehaviour);

            return poolBehaviour;
        }

        public void RefreshInstances() {
            // TODO We probably need to handle the fact that the starting
            // shouldn't be smaller than the maximum

            _instances = new List<PoolBehaviour>(_startingSize);
            while(_instances.Count < _startingSize) {
                CreateInstance();
            }
        }

        internal PoolBehaviour GetOrCreateInstance() {
            foreach(PoolBehaviour instance in _instances) {
                if(instance.IsGameObjectInactive || instance.Avalible) {
                    return instance;
                }
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
         * Internal Spawn Method.
         * Calls the internal spawn event but doesn't call spawn on
         */
        internal PoolBehaviour _Spawn() {
            return GetOrCreateInstance();
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

                // TODO Tidy this up
                if(_spawnPoint != null) { 
                    poolBehaviour.transform.position = _spawnPoint.position;
                }

                poolBehaviour._OnSpawn();
            }

            return poolBehaviour;
        }
    }
}