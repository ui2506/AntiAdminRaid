using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using System.Collections.Generic;

namespace AntiAdminRaid.EventHandlers
{
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
                        Extensions.UnbanPlayers(ipList, BanHandler.BanType.IP);
                    }

                    if (Plugin.PlayerUserId.TryGetValue(ev.Issuer, out List<string> idList))
                    {
                        Extensions.UnbanPlayers(idList, BanHandler.BanType.UserId);
                    }
                }

                ev.Issuer.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);

                Webhook.Send(Plugin.config.WebHookText.ValidateText(ev.Issuer));
            }

            Extensions.UpdatePlayerInfo(Plugin.PlayerUserId, ev.Issuer, ev.Player.UserId);
            Extensions.UpdatePlayerInfo(Plugin.PlayerIpAdress, ev.Issuer, ev.Player.IpAddress);

            Timing.CallDelayed(Plugin.config.BanCountKD, () => Plugin.AdminBanCount[ev.Issuer]--);
        }
    }
}
