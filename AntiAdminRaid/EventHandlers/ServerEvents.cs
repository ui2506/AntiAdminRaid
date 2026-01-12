using CommandSystem.Commands.RemoteAdmin;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AntiAdminRaid.EventHandlers
{
    internal sealed class ServerEvents
    {
        internal void Register()
        {
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.CommandExecuting += OnCommandExecuting;
        }
        
        internal void Unregister()
        {
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.CommandExecuting -= OnCommandExecuting;
        }

        private void OnRestartingRound() => BanInfo.Cache.Clear();

        private void OnCommandExecuting(CommandExecutingEventArgs ev)
        {
            if (!ev.Arguments.Any())
                return;

            Player player = ev.Sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if (player == null || player.UserGroup == null || player.UserGroup.Name == null)
                return;

            if (Plugin.PLuginConfig.IgnoredGroups.Contains(player.UserGroup.Name))
                return;

            switch (ev.Command)
            {
                case BanCommand _:
                    List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(ev.Arguments, 0, out string[] array, false);

                    if (list.Count < Plugin.PLuginConfig.SimultaneousBansCount)
                        break;

                    player.Ban(Plugin.PLuginConfig.RaidReason, Plugin.PLuginConfig.RaiderBanDuration * 86400);

                    Task.Run(() => Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(player)));

                    ev.IsAllowed = false;

                    break;

                case RconCommand _:
                    if (!Plugin.SudoCommandsBlackList.Contains(ev.Arguments.At(0).ToLower()))
                        break;

                    player.Ban(Plugin.PLuginConfig.RaidReason, Plugin.PLuginConfig.RaiderBanDuration * 86400);

                    Task.Run(() => Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(player)));

                    ev.IsAllowed = false;

                    break;
            }
        }
    }
}
