namespace AntiAdminRaid.Patches
{
    using System;
    using System.Collections.Generic;

    using CommandSystem;
    using CommandSystem.Commands.RemoteAdmin;

    using Exiled.API.Features;

    using HarmonyLib;

    using RemoteAdmin;

    using Utils;

    [HarmonyPatch(typeof(BanCommand), nameof(BanCommand.Execute))]

    internal static class BanCommandPatch
    {
        private static bool Prefix(BanCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if (player == Server.Host)
            {
                response = null;
                return true;
            }

            List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out string[] array, false);

            if (list.Count > Plugin.plugin.Config.SimultaneousBansCount)
            {
                Plugin.webhook.SendWebhook(Plugin.plugin.Config.WebHook, Plugin.plugin.Config.WebHookText.Replace("%nick%", player.Nickname).Replace("%steam%", player.UserId).Replace("%ip%", player.IPAddress));
                player.Ban(Plugin.plugin.Config.RaiderBanDuration * 86400, Plugin.plugin.Config.RaidReason);

                response = null;
                __result = false;
                return false;
            }

            response = null;
            return true;
        }
    }
}
