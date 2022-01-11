namespace BBUnity {

    /*
     * Allows you to intercept the OnSpawn / OnCreate events via an interface
     */
    public interface IPoolBehaviour {
        void OnCreate(PoolBehaviour behaviour);
        void OnSpawn(PoolBehaviour behaviour);
    }
}