using CutterManagement.Core;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CutterManagement.UI.Desktop
{
    public static class ServerRequest
    {
        public static async Task<HttpResponseMessage> ModifyAndSaveChanges<T>(HttpClient httpClient, string endPoint, Action<T> action) where T : class
        {
            var serverResponse = await httpClient.GetAsync(endPoint);
            var serverResponseContent = await serverResponse.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<T>(serverResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (serverResponse.IsSuccessStatusCode && item is not null)
            {
                action.Invoke(item);
            }
            else
            {
                return serverResponse;
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, MediaTypeNames.Application.Json);

            var serverUploadResult = await httpClient.PutAsync(endPoint, jsonContent);

            return serverUploadResult;
        }

        public static async Task<HttpResponseMessage> PutData<T>(HttpClient httpClient, string endPoint, T data) where T : class
        {
            var dataGoingToServer = JsonSerializer.Serialize(data);

            var jsonContent = new StringContent(dataGoingToServer, Encoding.UTF8, MediaTypeNames.Application.Json);

            var serverUploadResult = await httpClient.PutAsync(endPoint, jsonContent);
            serverUploadResult.EnsureSuccessStatusCode();

            return serverUploadResult;
        }

        public static async Task<HttpResponseMessage> PostData<T>(HttpClient httpClient, string endPoint, T data) where T : class
        {
            var dataGoingToServer = JsonSerializer.Serialize(data);

            var jsonContent = new StringContent(dataGoingToServer, Encoding.UTF8, MediaTypeNames.Application.Json);

            var serverUploadResult = await httpClient.PostAsync(endPoint, jsonContent);
            serverUploadResult.EnsureSuccessStatusCode();

            return serverUploadResult;
        }

        public static async Task<HttpResponseMessage> DeleteData<T>(HttpClient httpClient, string endPoint) where T : class
        {
            //var dataGoingToServer = JsonSerializer.Serialize(data);

            //var jsonContent = new StringContent(dataGoingToServer, Encoding.UTF8, "application/json");

            var serverUploadResult = await httpClient.DeleteAsync(endPoint);
            serverUploadResult.EnsureSuccessStatusCode();

            return serverUploadResult;
        }

        public static async Task<T?> GetData<T>(HttpClient httpClient, string endPoint) where T : class
        {
            var serverResponse = httpClient.GetAsync(endPoint).Result;
            var serverResponseContent = await serverResponse.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<T>(serverResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (serverResponse.IsSuccessStatusCode && item is not null)
            {
                return item;
            }

            return null;
        }

        public static async Task<List<T>?> GetDataCollection<T>(HttpClient httpClient, string endPoint) where T : class
        {
            var serverResponse = await httpClient.GetAsync(endPoint);
            var serverResponseContent = await serverResponse.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<List<T>>(serverResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (serverResponse.IsSuccessStatusCode && item is not null)
            {
                return item;
            }

            return null;
        }
    }
}
