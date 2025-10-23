using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace AntiAdminRaid.Patches
{
    [HarmonyPatch(typeof(RconCommand), nameof(RconCommand.Execute))]
    internal static class SudoCommandPatch
    {
        private static bool Prefix(RconCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            if (!arguments.Any())
            {
                response = null;
                return true;
            }

            Player player = Player.Get(sender);

            if (player.IsHost || Plugin.config.IgnoredGroups.Contains(player.UserGroup?.Name) || !Plugin.SudoCommandsBlackList.Contains(arguments.At(0).ToLower()))
            {
                response = null;
                return true;
            }

            player.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);

            Webhook.Send(Plugin.config.WebHookText.ValidateText(player));

            response = "Nope!";
            __result = false;
            return false;
        }
    }
}