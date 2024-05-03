using UnityEngine;

namespace BBUnity.Pools.Specialised {

    /// <summary>
    /// 
    /// </summary>
    public class EffectPool : StaticPool {

        const string DefaultSceneName = "Effect Pool";

        public static EffectPool DefaultInScene {
            get {
                foreach(EffectPool pool in FindObjectsOfType<EffectPool>()) {
                    if(string.Equals(pool.name, DefaultSceneName)) {
                        return pool;
                    }
                }

                Debug.LogError($"FindEffectPool.Find - Error finding default Effect Pool: { DefaultSceneName }");

                return null;
            }
        }
    }
}
