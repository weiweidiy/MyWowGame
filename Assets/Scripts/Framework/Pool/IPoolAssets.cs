namespace Framework.Pool
{
    /// <summary>
    /// 动态加载资源POOL接口
    /// </summary>
    public interface IPoolAssets
    {
        public string PoolObjName { get; set; }

        public void OnSpawn();
        public void OnRecycle();
    }
}