namespace AntiAdminRaid.EventHandlers
{
    using System.Collections.Generic;

    using Exiled.Events.EventArgs.Player;

    using MEC;

    internal class PlayerEvents
    {
        private readonly Plugin plugin;

        internal PlayerEvents(Plugin plugin)
        {
            this.plugin = plugin;
        }

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

        private void OnRestartingRound() => plugin.AdminBanCount.Clear();

        private void OnBanning(BanningEventArgs ev)
        {
            if (ev.Player.IsHost || ev.Player.IsNPC || ev.Player == null)
                return;

            if (!plugin.AdminBanCount.ContainsKey(ev.Player))
                plugin.AdminBanCount.Add(ev.Player, 0);

            plugin.AdminBanCount[ev.Player]++;

            if (plugin.AdminBanCount[ev.Player] >= plugin.Config.BanCount)
            {
                if (plugin.Config.UnBanPlayers)
                {
                    if (plugin.PlayerIpAdress.TryGetValue(ev.Player, out List<string> ipList))
                    {
                        plugin.UnbanPlayers(ipList, BanHandler.BanType.IP);
                    }

                    if (plugin.PlayerUserId.TryGetValue(ev.Player, out List<string> idList))
                    {
                        plugin.UnbanPlayers(idList, BanHandler.BanType.UserId);
                    }
                }

                ev.Player.Ban(plugin.Config.RaiderBanDuration * 86400, plugin.Config.RaidReason);

                Plugin.webhook.SendWebhook(
                    plugin.Config.WebHook,
                    plugin.Config.WebHookText
                        .Replace("%nick%", ev.Player.Nickname)
                        .Replace("%steam%", ev.Player.UserId)
                        .Replace("%ip%", ev.Player.IPAddress)
                );
            }

            plugin.UpdatePlayerInfo(plugin.PlayerUserId, ev.Player, ev.Target.UserId);
            plugin.UpdatePlayerInfo(plugin.PlayerIpAdress, ev.Player, ev.Target.IPAddress);

            Timing.CallDelayed(plugin.Config.BanCountKD, () => plugin.AdminBanCount[ev.Player]--);
        }
    }
}
