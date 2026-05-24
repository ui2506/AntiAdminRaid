using CommandSystem.Commands.RemoteAdmin;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace AntiAdminRaid.EventHandlers
{
    internal sealed class ServerEvents
    {
        internal void Register()
        {
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.CommandExecuting += OnCommandExecuting;
            LabApi.Events.Handlers.ServerEvents.BanIssuing += OnBanIssuing;
        }

        internal void Unregister()
        {
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.CommandExecuting -= OnCommandExecuting;
            LabApi.Events.Handlers.ServerEvents.BanIssuing -= OnBanIssuing;
        }

        private void OnBanIssuing(BanIssuingEventArgs ev)
        {
            string issuerId = ev.BanDetails.Issuer;

            if (issuerId.Contains("("))
                issuerId = ev.BanDetails.Issuer.Split('(')[1].Replace(")", "");

            Player issuer = Player.Get(issuerId);

            if (issuer == null || !issuer.IsPlayer)
                return;

            if (Plugin.PLuginConfig.IgnoredGroups.Contains(issuer.UserGroup?.Name))
                return;

            BanInfo.GetOrAdd(issuer, out BanInfo info);

            if (info.BanCount >= Plugin.PLuginConfig.BanCount)
            {
                if (Plugin.PLuginConfig.UnBanPlayers)
                    info.UnbanAll();

                _ = Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(issuer));

                issuer.Ban(Plugin.PLuginConfig.RaidReason, Plugin.PLuginConfig.RaiderBanDuration * 86400);

                ev.IsAllowed = false;

                return;
            }

            info.AddBan(ev.BanDetails.Id, ev.BanType == BanHandler.BanType.IP);
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

                    _ = Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(player));

                    ev.IsAllowed = false;

                    break;

                case RconCommand _:
                    if (!Plugin.SudoCommandsBlackList.Contains(ev.Arguments.At(0).ToLower()))
                        break;

                    player.Ban(Plugin.PLuginConfig.RaidReason, Plugin.PLuginConfig.RaiderBanDuration * 86400);

                    _ = Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(player));

                    ev.IsAllowed = false;

                    break;
            }
        }
    }
}
