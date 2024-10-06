using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Services.Clients
{
    public class WeSenderApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeSenderApiClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> SendSMSAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            var recipients = new List<string>() { phoneNumber };
            var payload = new
            {
                ApiKey = _apiKey,
                Destino = recipients,
                Mensagem = message,
                CEspeciais = "true"
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://api.wesender.co.ao/envio/apikey", content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (HttpRequestException ex)
            {
                // Handle exception
                Console.WriteLine($"Erro ao enviar SMS: {ex.Message}");
                return null;
            }
        }
    }

}
