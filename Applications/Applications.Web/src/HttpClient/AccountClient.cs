
using Applications.Web.src.HttpClients;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Applications.Web.src.HttpClients
{
    public class LoginResult
    {
        public string? Token { get; set; }
        public string? Error { get; set; }
    }

    public record LoginRequest(string Username, string Password)
    {
    }

    public class AccountClient(HttpClient httpClient, string username, string password)
    {
        public async Task<LoginResult> Login(CancellationToken cancellationToken = default)
        {
            LoginRequest req = new LoginRequest(username, password);
            var json = JsonSerializer.Serialize(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/login", content, cancellationToken);
            response.EnsureSuccessStatusCode(); // Lancia eccezione se status != 2xx

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<LoginResult>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result!;

        }
    }
}
