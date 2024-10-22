using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using MEC;
using System;
using System.Collections.Generic;

namespace AntiAdminRaid
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AntiRaid";
        public override string Name => "AntiRaid";
        public override string Author => "angelseraphim.";
        public override Version Version => new Version(1, 0, 0);

        public static Plugin plugin;
        public static Harmony harmony;
        public static Webhook webhook;

        private readonly Dictionary<Player, int> AdminBanCount = new Dictionary<Player, int>();
        private readonly Dictionary<Player, List<string>> PlayerUserId = new Dictionary<Player, List<string>>();
        private readonly Dictionary<Player, List<string>> PlayerIpAdress = new Dictionary<Player, List<string>>();

        public override void OnEnabled()
        {
            plugin = this;
            webhook = new Webhook();
            harmony = new Harmony("AntiRaid");
            Exiled.Events.Handlers.Player.Banning += OnBanning;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
            base.OnEnabled();
            harmony.PatchAll();
        }
        public override void OnDisabled ()
        {
            plugin = null;
            webhook = null;
            Exiled.Events.Handlers.Player.Banning -= OnBanning;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
            base.OnDisabled();
            harmony.UnpatchAll();
        }
        private void OnRestartingRound() => AdminBanCount.Clear();

        private void OnBanning(BanningEventArgs ev)
        {
            if (ev.Player.IsHost)
                return;

            if (!AdminBanCount.ContainsKey(ev.Player))
                AdminBanCount.Add(ev.Player, 0);

            AdminBanCount[ev.Player]++;

            if (AdminBanCount[ev.Player] >= Config.BanCount)
            {
                if (Config.UnBanPlayers)
                    UnbanPlayers(ev.Player);

                ev.Player.Ban(Config.RaiderBanDuration * 86400, Config.RaidReason);

                webhook.SendWebhook(
                    Config.WebHook,
                    Config.WebHookText
                        .Replace("%nick%", ev.Player.Nickname)
                        .Replace("%steam%", ev.Player.UserId)
                        .Replace("%ip%", ev.Player.IPAddress)
                );
            }

            UpdatePlayerInfo(PlayerUserId, ev.Player, ev.Target.UserId);
            UpdatePlayerInfo(PlayerIpAdress, ev.Player, ev.Target.IPAddress);

            Timing.CallDelayed(Config.BanCountKD, () => AdminBanCount[ev.Player]--);
        }

        private void UpdatePlayerInfo(Dictionary<Player, List<string>> dict, Player player, string info)
        {
            if (!dict.ContainsKey(player))
                dict[player] = new List<string>();

            dict[player].Add(info);
        }

        private void UnbanPlayers(Player player)
        {
            if (PlayerUserId.TryGetValue(player, out List<string> idList))
            {
                foreach(string id in idList)
                {
                    Server.ExecuteCommand($"/unban id {id}");
                }
            }
            if (PlayerIpAdress.TryGetValue(player, out List<string> ipList))
            {
                foreach (string ip in ipList)
                {
                    Server.ExecuteCommand($"/unban ip {ip}");
                }
            }
        }
    }
}
