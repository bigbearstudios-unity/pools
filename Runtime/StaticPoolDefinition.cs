using UnityEngine;

using BBUnity.Pools;

namespace BBUnity {

    [System.Serializable]
    public class StaticPoolDefinition : BasePoolDefinition {

        public StaticPoolDefinition() : base() {}
        public StaticPoolDefinition(GameObject prefab) : base(prefab) {}
        public StaticPoolDefinition(string name, GameObject prefab) : base(name, prefab) {}
        public StaticPoolDefinition(GameObject prefab, int defaultSize) : base(prefab, defaultSize) {}
        public StaticPoolDefinition(string name, GameObject prefab, int defaultSize) : base(name, prefab, defaultSize) {}
        public StaticPoolDefinition(GameObject prefab, int defaultSize, int maximumSize) : base(prefab, defaultSize, maximumSize) { }
        public StaticPoolDefinition(string name, GameObject prefab, int defaultSize, int maximumSize) : base(name, prefab, defaultSize, maximumSize) {}
    }
}
