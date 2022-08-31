

namespace RentACar.Redis.CachingModels
{
    public interface IRedisCaching<T> where T : class
    {
        void AddCache();
        List<T> GetCacheDataList();
    }
}
