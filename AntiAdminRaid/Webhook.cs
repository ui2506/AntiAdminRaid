namespace AntiAdminRaid
{
    using Exiled.API.Features;
    using System.Net.Http;
    using System.Text;

    internal static class Webhook
    {
        internal static async void SendWebhook(string webhookUrl, string message)
        {
            if (string.IsNullOrEmpty(webhookUrl))
            {
                Log.Error("Webhook url is empty");
                return;
            }

            using (var httpClient = new HttpClient())
            {
                var payload = new
                {
                    content = message
                };

                var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(webhookUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Log.Debug("Message sent successfully!");
                }
                else
                {
                    Log.Error($"Failed to send message. Status code: {response.StatusCode}");
                }
            }
        }
    }
}
