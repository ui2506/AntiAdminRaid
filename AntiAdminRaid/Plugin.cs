namespace AntiAdminRaid
{
    using AntiAdminRaid.EventHandlers;
    using Exiled.API.Features;
    using HarmonyLib;
    using System.Collections.Generic;

    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AntiAdminRaid";
        public override string Name => "AntiAdminRaid";
        public override string Author => "angelseraphim.";

        internal static readonly Dictionary<Player, int> AdminBanCount = new Dictionary<Player, int>();
        internal static readonly Dictionary<Player, List<string>> PlayerUserId = new Dictionary<Player, List<string>>();
        internal static readonly Dictionary<Player, List<string>> PlayerIpAdress = new Dictionary<Player, List<string>>();

        internal static Config config;

        private PlayerEvents playerEvents;
        private Harmony harmony;

        public override void OnEnabled()
        {
            config = Config;
            harmony = new Harmony("AntiRaid");
            playerEvents = new PlayerEvents();

            harmony.PatchAll();
            playerEvents.Register();

            base.OnEnabled();
        }

        public override void OnDisabled ()
        {
            harmony.UnpatchAll();
            playerEvents.Unregister();

            config = null;
            harmony = null;
            playerEvents = null;

            base.OnDisabled();
        }

        public override void OnReloaded()
        {
            OnDisabled();
            OnEnabled();

            base.OnReloaded();
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
