using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AntiAdminRaid.Patches
{
    [HarmonyPatch(typeof(BanCommand), nameof(BanCommand.Execute))]
    internal static class BanCommandPatch
    {
        private static bool Prefix(ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
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
                player.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);

                Task.Run(() => Webhook.Send(Plugin.config.WebHookText.ValidateText(player)));

                response = null;
                __result = false;
                return false;
            }

            response = null;
            return true;
        }
    }
}
