using AntiAdminRaid.EventHandlers;
using LabApi.Loader.Features.Plugins;
using System;

namespace AntiAdminRaid
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "AntiAdminRaid";
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = "AntiAdminRaid";
        public override Version Version { get; } = new Version(2, 4, 1);
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 4);

        internal static readonly string[] SudoCommandsBlackList = new string[] { "stop", "exit" };

        internal static Config PLuginConfig { get; private set; }

        private ServerEvents _serverEvents;

        public override void Enable()
        {
            PLuginConfig = Config;
            _serverEvents = new ServerEvents();

            _serverEvents.Register();
        }

        public override void Disable()
        {
            _serverEvents.Unregister();

            PLuginConfig = null;
            _serverEvents = null;
        }
    }
}
