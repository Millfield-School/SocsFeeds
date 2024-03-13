using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocsFeeds.helpers
{
    public static class ApiClientProvider
    {
        public static string ApiKey = string.Empty;
        public static string ApiId = string.Empty;
        public static string BaseUrl = "https://www.socscms.com/socs/xml/";
        private static readonly HttpClient _httpClient = CreateClient();

        private static HttpClient CreateClient()
        {
            var apiClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            return apiClient;
        }

        public static async Task<bool> IsApiUpAsync()
        {
            string url = "https://www.schoolssports.com/school/xml/fixturecalendar.ashx";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);
                    return response.IsSuccessStatusCode;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error checking API status: {e.Message}");
                    return false;
                }
            }
        }

        public static async Task<HttpResponseMessage> GetApiResponseAsync(string endpoint, Dictionary<string, string> additionalParameters = null)
        {
            // Read settings from configuration
            // Base query parameters (you might always need ID and key)
            var queryParameters = new Dictionary<string, string>
            {
                {"ID", ApiId},
                {"key", ApiKey}
            };

            // If there are additional parameters, add them to the queryParameters dictionary
            if (additionalParameters != null)
            {
                foreach (var param in additionalParameters)
                {
                    queryParameters[param.Key] = param.Value;
                }
            }

            if (endpoint.Equals("fixturecalendar") || endpoint.Equals("results") || endpoint.Equals("mso-sport"))
                _httpClient.BaseAddress = new Uri("https://www.schoolssports.com/school/xml/");
            else
                _httpClient.BaseAddress = new Uri(BaseUrl);

            // Build the query string
            var queryString = await new FormUrlEncodedContent(queryParameters).ReadAsStringAsync();
            var requestUri = $"{_httpClient.BaseAddress}{endpoint}.ashx?{queryString}";
            return await _httpClient.GetAsync(requestUri);
        }

        public static HttpClient Client => _httpClient;
    }
}
