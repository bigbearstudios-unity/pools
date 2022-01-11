using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Pools.Internal {

    [System.Serializable]
    public class BasePoolDefinition {

        public delegate void OnSpawnEventHandler(PoolBehaviour poolBehaviour);

        /// <summary>
        /// The default maximum size of the pool definition
        /// </summary>
        public const int DefaultMaximumSize = 500;

        /// <summary>
        /// The default starting size of the pool definition,
        /// E.g. How many of the PoolBehaviours are spawned upon Awake
        /// </summary>
        public const int DefaultStartingSize = 0;

        [SerializeField, Tooltip("The name of the Definition. This is used to access the definition, defaults to the prefab name")]
        private string _name = null;

        [SerializeField, Tooltip("The prefab which will be instanciated")]
        private GameObject _prefab = null;

        [SerializeField, Tooltip("The starting size of the pool. This will be filled with instanciated prefabs")]
        private int _startingSize = DefaultStartingSize;

        [SerializeField, Tooltip("The maximum size of the pool.`")]
        private int _maximumSize = DefaultMaximumSize;

        [Tooltip("Should we use disabled instances as well as 'avalible' instances")]
        [SerializeField]
        private bool _useDisabledInstances = true;

        private List<PoolBehaviour> _instances = null;
        private Transform _defaultParent = null;
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

        public BasePoolDefinition() { }

        public BasePoolDefinition(GameObject prefab) {
            SetPrefab(prefab);
        }

        public BasePoolDefinition(string name, GameObject prefab) {
            SetName(name);
            SetPrefab(prefab);
        }

        public BasePoolDefinition(GameObject prefab, int startingSize) {
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            RefreshInstances();
        }

        public BasePoolDefinition(string name, GameObject prefab, int startingSize) {
            SetName(name);
            SetPrefab(prefab);
            SetStartingSize(startingSize);
            RefreshInstances();
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

        public void RefreshInstances() {
            _instances = new List<PoolBehaviour>(_startingSize);
            while(_instances.Count < _startingSize) {
                CreateInstance();
            }
        }

        internal PoolBehaviour GetOrCreateInstance() {
            if(_useDisabledInstances) {
                foreach(PoolBehaviour instance in _instances) {
                    if(instance.Inactive || instance.Avalible) {
                        return instance;
                    }
                }
            } else {
                foreach(PoolBehaviour instance in _instances) {
                    if(instance.Avalible) {
                        return instance;
                    }
                }
            }
            
            if(AllowGrowth) {
                return CreateInstance();
            }

            return null;
        }

        private PoolBehaviour InstantiateInstance() {
            PoolBehaviour poolBehaviour = Utilities.InstantiateWithComponent<PoolBehaviour>(_prefab, _defaultParent, true);
            poolBehaviour._OnCreate(this);
            return poolBehaviour;
        }

        private PoolBehaviour CreateInstance() {
            return Utilities.Tap(InstantiateInstance(), (PoolBehaviour instance) => {
                _instances.Add(instance);
            });
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
                poolBehaviour._OnSpawn();
            }

            return poolBehaviour;
        }
    }
}