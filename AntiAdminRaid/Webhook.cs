namespace AntiAdminRaid
{
    using LabApi.Features.Console;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Text;

    internal static class Webhook
    {
        internal static async void SendWebhook(string webhookUrl, string message)
        {
            if (string.IsNullOrEmpty(webhookUrl))
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

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(webhookUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Logger.Debug("Message sent successfully!");
                }
                else
                {
                    Logger.Error($"Failed to send message. Status code: {response.StatusCode}");
                }
            }
        }
    }
}
