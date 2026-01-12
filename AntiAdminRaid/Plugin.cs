using AntiAdminRaid.EventHandlers;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;

namespace AntiAdminRaid
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "AntiAdminRaid";
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = "AntiAdminRaid";
        public override Version Version { get; } = new Version(2, 3, 0);
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 4);

        internal static readonly string[] SudoCommandsBlackList = new string[] { "stop", "exit" };
        internal static readonly Dictionary<Player, BanInfo> BanInfo = new Dictionary<Player, BanInfo>();

        internal static Config PLuginConfig { get; private set; }

        private PlayerEvents playerEvents;
        private ServerEvents serverEvents;

        public override void Enable()
        {
            PLuginConfig = Config;
            playerEvents = new PlayerEvents();
            serverEvents = new ServerEvents();

            playerEvents.Register();
            serverEvents.Register();
        }

        public override void Disable()
        {
            playerEvents.Unregister();
            serverEvents.Unregister();

            PLuginConfig = null;
            playerEvents = null;
            serverEvents = null;
        }
    }
}
