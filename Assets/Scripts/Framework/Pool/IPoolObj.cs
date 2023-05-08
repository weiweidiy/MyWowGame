namespace Framework.Pool
{
    /// <summary>
    /// GO POOL 通用接口
    /// </summary>
    public interface IPoolObj
    {
        public void OnSpawn();
        public void OnRecycle();
    }
}