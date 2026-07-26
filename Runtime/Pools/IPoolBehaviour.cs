namespace BBUnity.Pools {

    /// <summary>
    /// Implement this on a component living on the pooled prefab itself — it's discovered and
    /// wired up automatically by PoolBehaviour, no manual subscription needed. If the calling
    /// code doesn't own the prefab (e.g. it just called ObjectPool.Spawn() from elsewhere),
    /// use PoolBehaviour's OnCreateEvent/OnSpawnEvent/OnReturnEvent instead.
    /// </summary>
    public interface IPoolBehaviour {
        void OnCreate(PoolBehaviour behaviour);
        void OnSpawn(PoolBehaviour behaviour);
        void OnReturn(PoolBehaviour behaviour);
    }
}
