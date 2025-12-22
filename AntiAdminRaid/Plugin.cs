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
        public override string Name { get; } = "AntiAdminRaid";
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = "AntiAdminRaid";
        public override Version Version { get; } = new Version(2, 2, 1);
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 4);

        internal static readonly string[] SudoCommandsBlackList = new string[] { "stop", "exit" };
        internal static readonly Dictionary<Player, BanInfo> BanInfo = new Dictionary<Player, BanInfo>();

        internal static Config config { get; private set; }

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
