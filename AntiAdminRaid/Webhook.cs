using LabApi.Features.Console;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;

namespace AntiAdminRaid
{
    internal static class Webhook
    {
        internal static async Task Send(string message)
        {
            if (string.IsNullOrEmpty(Plugin.config.WebHook))
            {
                Logger.Error("Webhook url is empty");
                return;
            }

            using (var httpClient = new HttpClient())
            {
                var payload = new
                {
                    content = message
                };

                byte[] jsonBytes = JsonSerializer.Serialize(payload);
                string jsonPayload = Encoding.UTF8.GetString(jsonBytes);
                StringContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(Plugin.config.WebHook, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Logger.Debug("Message sent successfully!", Plugin.config.Debug);
                }
                else
                {
                    Logger.Error($"Failed to send message. Status code: {response.StatusCode}");
                }
            }
        }
    }
}
