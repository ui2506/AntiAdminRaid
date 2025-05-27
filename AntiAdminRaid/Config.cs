namespace AntiAdminRaid
{
    using System.ComponentModel;

    public class Config
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string WebHook { get; set; } = "https://discord.com/api/webhooks/1221525654630174841/gr8zvsulfQnKhhc53JUu7Mp6noFs4XAAX-gfmBddQe5lU6SBgkhvbybE-ulPQ_ckGYME";

        [Description("Maximum bans number")]
        public int BanCount { get; set; } = 3;

        [Description("How long will it take for the ban k/d to decrease?")]
        public int BanCountKD { get; set; } = 60;

        [Description("Unban all players who were banned by the raider")]
        public bool UnBanPlayers { get; set; } = true;

        [Description("Maximum number of simultaneous bans")]
        public int SimultaneousBansCount { get; set; } = 3;

        [Description("Raider ban reason")]
        public string RaidReason { get; set; } = "Ugh...";

        [Description("Raider ban duration (in days)")]
        public int RaiderBanDuration { get; set; } = 7;

        [Description("%nick% - admin name, %steam% - admin SteamID64@steam, %ip% - admin ip adress")]
        public string WebHookText { get; set; } = "**%nick%** (%steam%)[%ip%] :name_badge: был заблокирован по подозрению на рейд!";
    }
}
