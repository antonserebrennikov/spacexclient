namespace Game.Utils.ObjectPool
{
    public interface IObjectPool<T>
    {
        T GetFromPool();
        void ReturnToPool(T obj);
        int Count { get; }
    }
}