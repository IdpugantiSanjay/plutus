using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Plutus.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TR?> PostAsync<TR>(this HttpClient client, string requestUri, object content)
        {
            var httpResponse = await client.PostAsync(requestUri, JsonContent.Create(content));
            return await httpResponse.Content.ReadFromJsonAsync<TR>();
        }
    }
}