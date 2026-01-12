using System.ComponentModel;

namespace AntiAdminRaid
{
    public sealed class Config
    {
        [Description("Debug mode")]
        public bool Debug { get; set; } = false;

        [Description("Discord webhook")]
        public string WebHook { get; set; } = string.Empty;

        [Description("Maximum bans number 1-255")]
        public byte BanCount { get; set; } = 3;

        [Description("How long will it take for the ban k/d to decrease? 1-65535")]
        public ushort BanCountKD { get; set; } = 60;

        [Description("Unban all players who were banned by the raider")]
        public bool UnBanPlayers { get; set; } = true;

        [Description("Maximum number of simultaneous bans 1-255")]
        public byte SimultaneousBansCount { get; set; } = 3;

        [Description("Raider ban reason")]
        public string RaidReason { get; set; } = "Ugh...";

        [Description("Raider ban duration (in days 1-65535)")]
        public ushort RaiderBanDuration { get; set; } = 7;

        [Description("Groups to be ignored")]
        public string[] IgnoredGroups { get; set; } = new string[] { "owner" };

        [Description("%nick% - admin name, %steam% - admin SteamID64@steam, %ip% - admin ip adress")]
        public string WebHookText { get; set; } = "**%nick%** (%steam%)[%ip%] :name_badge: has been banned!";
    }
}
