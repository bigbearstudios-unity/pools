namespace BBUnity.Pools {

    /// <summary>
    /// 
    /// </summary>
    public interface IPoolBehaviour {
        void OnCreate(PoolBehaviour behaviour);
        void OnSpawn(PoolBehaviour behaviour);
    }
}