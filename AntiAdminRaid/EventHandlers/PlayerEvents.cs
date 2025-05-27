namespace AntiAdminRaid.EventHandlers
{
    using LabApi.Events.Arguments.PlayerEvents;
    using MEC;
    using System.Collections.Generic;

    internal class PlayerEvents
    {
        internal void Register()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning += OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
        }

        internal void Unregister()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning -= OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;
        }

        private void OnRestartingRound() => Plugin.AdminBanCount.Clear();

        private void OnBanning(PlayerBanningEventArgs ev)
        {
            if (ev.Issuer.IsHost || ev.Issuer.IsDummy || ev.Issuer == null)
                return;

            if (!Plugin.AdminBanCount.ContainsKey(ev.Issuer))
                Plugin.AdminBanCount.Add(ev.Issuer, 0);

            Plugin.AdminBanCount[ev.Issuer]++;

            if (Plugin.AdminBanCount[ev.Issuer] >= Plugin.config.BanCount)
            {
                if (Plugin.config.UnBanPlayers)
                {
                    if (Plugin.PlayerIpAdress.TryGetValue(ev.Issuer, out List<string> ipList))
                    {
                        Plugin.UnbanPlayers(ipList, BanHandler.BanType.IP);
                    }

                    if (Plugin.PlayerUserId.TryGetValue(ev.Issuer, out List<string> idList))
                    {
                        Plugin.UnbanPlayers(idList, BanHandler.BanType.UserId);
                    }
                }

                ev.Issuer.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);

                Webhook.SendWebhook(
                    Plugin.config.WebHook,
                    Plugin.config.WebHookText
                        .Replace("%nick%", ev.Issuer.Nickname)
                        .Replace("%steam%", ev.Issuer.UserId)
                        .Replace("%ip%", ev.Issuer.IpAddress)
                );
            }

            Plugin.UpdatePlayerInfo(Plugin.PlayerUserId, ev.Player, ev.Player.UserId);
            Plugin.UpdatePlayerInfo(Plugin.PlayerIpAdress, ev.Player, ev.Player.IpAddress);

            Timing.CallDelayed(Plugin.config.BanCountKD, () => Plugin.AdminBanCount[ev.Issuer]--);
        }
    }
}
