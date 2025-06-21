using AntiAdminRaid.EventHandlers;
using HarmonyLib;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;

namespace AntiAdminRaid
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "AntiAdminRaid";
        public override string Author => "angelseraphim.";
        public override string Description => "AntiAdminRaid";
        public override Version Version => new Version(2, 1, 0);
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
            harmony = new Harmony(Name);
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
    }
}
