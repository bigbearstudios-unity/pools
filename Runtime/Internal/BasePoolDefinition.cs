using UnityEngine;
using System.Collections.Generic;

namespace BBUnity.Pools.Internal {

    [System.Serializable]
    public class BasePoolDefinition {

        public delegate void OnSpawnHandler(PoolBehaviour poolBehaviour);

        /// <summary>
        /// The default maximum size of the pool definition
        /// </summary>
        public const int DefaultMaximumSize = 500;

        /// <summary>
        /// The default starting size of the pool definition,
        /// E.g. How many of the PoolBehaviours are spawned upon Awake
        /// </summary>
        public const int DefaultStartingSize = 0;

        [SerializeField]
        private string _name = null;

        [SerializeField]
        private GameObject _prefab = null;

        [SerializeField]
        private int _startingSize = DefaultStartingSize;

        [SerializeField]
        private int _maximumSize = DefaultMaximumSize;

        [Tooltip("Should we use disabled instances as well as 'avalible' instances")]
        [SerializeField]
        private bool _useDisabledInstances = true;

        /// <summary>
        /// The instances which belong to the pool definition
        /// </summary>
        private List<PoolBehaviour> _instances = null;

        /// <summary>
        /// The parent which will be applied to each instance by default (This can be overridden)
        /// </summary>
        private Transform _defaultParent = null;

        public event OnSpawnHandler OnSpawnEvent;

        public string Name {
            get { return (_name != null && _name.Length > 0) ? _name : _prefab.name; }
            set { _name = value; }
        }

        public GameObject Prefab {
            get { return _prefab; }
            set { _prefab = value; }
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
            get {
                if(_prefab == null) {
                    Debug.LogError("BasePoolDefinition.Validate - No prefab to instantiate");
                    return false;
                }

                return true;
            }
        }

        public BasePoolDefinition() {
        
        }

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
                AddNewInstance();
            }
        }

        private PoolBehaviour GetOrCreateInstance() {
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
                return AddNewInstance();
            }

            return null;
        }

        private PoolBehaviour CreateInstance() {
            PoolBehaviour poolBehaviour = Utilities.InstantiateWithComponent<PoolBehaviour>(_prefab, _defaultParent);
            poolBehaviour._OnCreate(this);
            return poolBehaviour;
        }

        private PoolBehaviour AddNewInstance() {
            return Utilities.Tap(CreateInstance(), (PoolBehaviour instance) => {
                _instances.Add(instance);
            });
        }

        /*
         * Public Spawn Methods
         */

        public PoolBehaviour Spawn() {
            PoolBehaviour poolBehaviour = GetOrCreateInstance();
            if (poolBehaviour != null) {
                poolBehaviour._OnSpawn();
                OnSpawnEvent?.Invoke(poolBehaviour);
            }

            return poolBehaviour;
        }

        public PoolBehaviour Spawn(System.Action<PoolBehaviour> beforeSpawn, System.Action<PoolBehaviour> afterSpawn) {
            PoolBehaviour poolBehaviour = GetOrCreateInstance();
            if (poolBehaviour != null) {
                beforeSpawn(poolBehaviour);
                poolBehaviour._OnSpawn();
                OnSpawnEvent?.Invoke(poolBehaviour);
                afterSpawn(poolBehaviour);
            }

            return poolBehaviour;
        }

        public PoolBehaviour Spawn(Transform parent) {
            PoolBehaviour poolBehaviour = GetOrCreateInstance();
            if (poolBehaviour != null) {
                poolBehaviour.SetParent(parent);
                poolBehaviour._OnSpawn();
                OnSpawnEvent?.Invoke(poolBehaviour);
            }

            return poolBehaviour;
        }
    }
}