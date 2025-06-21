using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace AntiAdminRaid
{
    internal static class Extensions
    {
        internal static string ValidateText(this string text, Player player)
        {
            return text
                .Replace("%nick%", player.Nickname)
                .Replace("%steam%", player.UserId)
                .Replace("%ip%", player.IpAddress)
                .Replace("%badge_text%", player.UserGroup.BadgeText);
        }

        internal static void UpdatePlayerInfo(Dictionary<Player, List<string>> dict, Player player, string info)
        {
            if (!dict.ContainsKey(player))
                dict[player] = new List<string>();

            dict[player].Add(info);
        }

        internal static void UnbanPlayers(IEnumerable<string> list, BanHandler.BanType banType)
        {
            foreach (string item in list)
            {
                BanHandler.RemoveBan(item, banType);
            }
        }
    }
}
