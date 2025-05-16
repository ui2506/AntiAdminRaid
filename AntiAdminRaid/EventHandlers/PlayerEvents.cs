namespace AntiAdminRaid.EventHandlers
{
    using Exiled.Events.EventArgs.Player;
    using MEC;
    using System.Collections.Generic;

    internal class PlayerEvents
    {
        internal void Register()
        {
            Exiled.Events.Handlers.Player.Banning += OnBanning;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
        }

        internal void Unregister()
        {
            Exiled.Events.Handlers.Player.Banning -= OnBanning;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
        }

        private void OnRestartingRound() => Plugin.AdminBanCount.Clear();

        private void OnBanning(BanningEventArgs ev)
        {
            if (ev.Player.IsHost || ev.Player.IsNPC || ev.Player == null)
                return;

            if (!Plugin.AdminBanCount.ContainsKey(ev.Player))
                Plugin.AdminBanCount.Add(ev.Player, 0);

            Plugin.AdminBanCount[ev.Player]++;

            if (Plugin.AdminBanCount[ev.Player] >= Plugin.config.BanCount)
            {
                if (Plugin.config.UnBanPlayers)
                {
                    if (Plugin.PlayerIpAdress.TryGetValue(ev.Player, out List<string> ipList))
                    {
                        Plugin.UnbanPlayers(ipList, BanHandler.BanType.IP);
                    }

                    if (Plugin.PlayerUserId.TryGetValue(ev.Player, out List<string> idList))
                    {
                        Plugin.UnbanPlayers(idList, BanHandler.BanType.UserId);
                    }
                }

                ev.Player.Ban(Plugin.config.RaiderBanDuration * 86400, Plugin.config.RaidReason);

                Webhook.SendWebhook(
                    Plugin.config.WebHook,
                    Plugin.config.WebHookText
                        .Replace("%nick%", ev.Player.Nickname)
                        .Replace("%steam%", ev.Player.UserId)
                        .Replace("%ip%", ev.Player.IPAddress)
                );
            }

            Plugin.UpdatePlayerInfo(Plugin.PlayerUserId, ev.Player, ev.Target.UserId);
            Plugin.UpdatePlayerInfo(Plugin.PlayerIpAdress, ev.Player, ev.Target.IPAddress);

            Timing.CallDelayed(Plugin.config.BanCountKD, () => Plugin.AdminBanCount[ev.Player]--);
        }
    }
}
