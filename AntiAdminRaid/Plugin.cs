namespace AntiAdminRaid
{
    using AntiAdminRaid.EventHandlers;
    using HarmonyLib;
    using LabApi.Features.Wrappers;
    using LabApi.Loader.Features.Plugins;
    using System;
    using System.Collections.Generic;

    public class Plugin : Plugin<Config>
    {
        public override string Name => "AntiAdminRaid";
        public override string Author => "angelseraphim.";
        public override string Description => "AntiAdminRaid";
        public override Version Version => new Version(2, 0, 0);
        public override Version RequiredApiVersion => new Version(1, 0, 2);

        internal static readonly Dictionary<Player, int> AdminBanCount = new Dictionary<Player, int>();
        internal static readonly Dictionary<Player, List<string>> PlayerUserId = new Dictionary<Player, List<string>>();
        internal static readonly Dictionary<Player, List<string>> PlayerIpAdress = new Dictionary<Player, List<string>>();

        internal static Config config;

        private PlayerEvents playerEvents;
        private ServerEvents serverEvents;
        private Harmony harmony;

        public override void Enable()
        {
            config = Config;
            harmony = new Harmony("AntiRaid");
            playerEvents = new PlayerEvents();
            serverEvents = new ServerEvents();

            harmony.PatchAll();
            playerEvents.Register();
            serverEvents.Register();
        }

        public override void Disable()
        {
            harmony.UnpatchAll();
            playerEvents.Unregister();
            serverEvents.Unregister();

            config = null;
            harmony = null;
            playerEvents = null;
            serverEvents = null;
        }

        internal static void UpdatePlayerInfo(Dictionary<Player, List<string>> dict, Player player, string info)
        {
            if (!dict.ContainsKey(player))
                dict[player] = new List<string>();

            dict[player].Add(info);
        }

        internal static void UnbanPlayers(List<string> list, BanHandler.BanType banType)
        {
            foreach (string item in list)
            {
                BanHandler.RemoveBan(item, banType);
            }
        }
    }
}
