using LabApi.Features.Wrappers;

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
                .Replace("%badge_text%", player.UserGroup?.BadgeText ?? string.Empty);
        }
    }
}
