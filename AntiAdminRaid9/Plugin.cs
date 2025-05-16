namespace AntiAdminRaid
{
    using System.Collections.Generic;

    using AntiAdminRaid.EventHandlers;

    using Exiled.API.Features;

    using HarmonyLib;

    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AntiRaid";
        public override string Name => "AntiRaid";
        public override string Author => "angelseraphim.";

        internal static Plugin plugin;
        internal static Harmony harmony;
        internal static Webhook webhook;

        private PlayerEvents playerEvents;

        internal readonly Dictionary<Player, int> AdminBanCount = new Dictionary<Player, int>();
        internal readonly Dictionary<Player, List<string>> PlayerUserId = new Dictionary<Player, List<string>>();
        internal readonly Dictionary<Player, List<string>> PlayerIpAdress = new Dictionary<Player, List<string>>();

        public override void OnEnabled()
        {
            plugin = this;
            webhook = new Webhook();
            harmony = new Harmony("AntiRaid");
            playerEvents = new PlayerEvents(this);

            harmony.PatchAll();
            playerEvents.Register();

            base.OnEnabled();
        }

        public override void OnDisabled ()
        {
            harmony.UnpatchAll();
            playerEvents.Unregister();

            plugin = null;
            webhook = null;
            harmony = null;
            playerEvents = null;

            base.OnDisabled();
        }

        internal void UpdatePlayerInfo(Dictionary<Player, List<string>> dict, Player player, string info)
        {
            if (!dict.ContainsKey(player))
                dict[player] = new List<string>();

            dict[player].Add(info);
        }

        internal void UnbanPlayers(List<string> list, BanHandler.BanType banType)
        {
            foreach (string item in list)
            {
                BanHandler.RemoveBan(item, banType);
            }
        }
    }
}
