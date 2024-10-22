using CommandSystem.Commands.RemoteAdmin;
using CommandSystem;
using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace AntiAdminRaid
{
    [HarmonyPatch(typeof(BanCommand), nameof(BanCommand.Execute))]

    public class BanCommandPatch
    {
        static bool Prefix(BanCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            Player player = Player.Get(sender);
            if (player != null && !player.IsHost)
            {
                List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out string[] array, false);
                if (list.Count() > Plugin.plugin.Config.SimultaneousBansCount)
                {
                    Plugin.webhook.SendWebhook(Plugin.plugin.Config.WebHook, Plugin.plugin.Config.WebHookText.Replace("%nick%", player.Nickname).Replace("%steam%", player.UserId).Replace("%ip%", player.IPAddress));
                    player.Ban(Plugin.plugin.Config.RaiderBanDuration * 86400, Plugin.plugin.Config.RaidReason);
                    response = null;
                    __result = false;
                    return false;
                }
            }
            response = null;
            return true;
        }
    }
}
