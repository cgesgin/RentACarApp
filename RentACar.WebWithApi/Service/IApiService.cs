namespace RentACar.WebWithApi.Service
{
    public interface IApiService
    {
        Task<List<T>> GetAllAsync<T>(string url) where T : class;
        Task<T> SaveAsync<T>(string url, T data) where T : class;
        Task<bool> UpdateAsync<T>(string url, T data) where T : class;
        Task<T> GetByIdAsync<T>(string url) where T : class;
        Task<bool> RemoveAsync(string url);
    }
}
