using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace AntiAdminRaid.Patches
{
    [HarmonyPatch(typeof(BanCommand), nameof(BanCommand.Execute))]
    internal static class BanCommandPatch
    {
        private static bool Prefix(BanCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            if (!arguments.Any())
            {
                response = null;
                return true;
            }

            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if (player == Server.Host)
            {
                response = null;
                return true;
            }

            List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out string[] array, false);

            if (list.Count > Plugin.config.SimultaneousBansCount)
            {
                Webhook.Send(Plugin.config.WebHookText.ValidateText(player));
                player.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);

                response = null;
                __result = false;
                return false;
            }

            response = null;
            return true;
        }
    }
}
