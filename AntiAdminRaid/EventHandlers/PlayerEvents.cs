using LabApi.Events.Arguments.PlayerEvents;
using System.Threading.Tasks;

namespace AntiAdminRaid.EventHandlers
{
    internal sealed class PlayerEvents
    {
        internal void Register()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning += OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRoundRestarted;
        }

        internal void Unregister()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning -= OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRoundRestarted;
        }

        private void OnRoundRestarted() => Plugin.BanInfo.Clear();

        private void OnBanning(PlayerBanningEventArgs ev)
        {
            if (ev.Issuer.IsHost || ev.Issuer.IsDummy)
                return;

            if (Plugin.PLuginConfig.IgnoredGroups.Contains(ev.Player.UserGroup?.Name))
                return;

            if (!Plugin.BanInfo.TryGetValue(ev.Issuer, out BanInfo info))
            {
                info = new BanInfo();
                Plugin.BanInfo[ev.Issuer] = info;
            }

            if (info.BanCount >= Plugin.PLuginConfig.BanCount)
            {
                if (Plugin.PLuginConfig.UnBanPlayers)
                    info.UnbanAll();

                Task.Run(() => Webhook.Send(Plugin.PLuginConfig.WebHookText.ValidateText(ev.Issuer)));

                ev.Issuer.Ban(Plugin.PLuginConfig.RaidReason, Plugin.PLuginConfig.RaiderBanDuration * 86400);
                ev.IsAllowed = false;

                return;
            }

            info.AddBan(ev.Player.UserId, ev.Player.IpAddress);
        }
    }
}
