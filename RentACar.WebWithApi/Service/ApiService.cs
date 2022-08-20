using Newtonsoft.Json;
using RentACar.Core.DTOs;
using System.Net.Http.Json;
using System.Text;

namespace RentACar.WebWithApi.Service
{
    public class ApiService
    {
        public readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<T>> GetAllAsync<T>(string url) where T : class
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<T>>>(url);
            return response.Data;
        }

        public async Task<T> SaveAsync<T>(string url,T data) where T : class 
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode) 
            {
                dynamic responseBody = await response.Content.ReadFromJsonAsync<ResponseDto<T>>();
                return responseBody.Data;
            }
            return null;
        }
        
        public async Task<bool> UpdateAsync<T>(string url, T data) where T : class
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<T> GetByIdAsync<T>(string url) where T : class//$"costumers/{id}"
        {
            dynamic response = await _httpClient.GetFromJsonAsync<ResponseDto<T>>(url);
            return response.Data;
        }

        public async Task<bool> RemoveAsync(string url)
        {
            dynamic response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
