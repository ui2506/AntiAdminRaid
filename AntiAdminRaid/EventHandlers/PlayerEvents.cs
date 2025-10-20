using LabApi.Events.Arguments.PlayerEvents;

namespace AntiAdminRaid.EventHandlers
{
    internal class PlayerEvents
    {
        internal void Register()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning += OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
        }

        internal void Unregister()
        {
            LabApi.Events.Handlers.PlayerEvents.Banning -= OnBanning;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;
        }

        private void OnRestartingRound() => Plugin.BanInfo.Clear();

        private void OnBanning(PlayerBanningEventArgs ev)
        {
            if (ev.Issuer.IsHost || ev.Issuer.IsDummy || ev.Issuer == null)
                return;

            if (!Plugin.BanInfo.TryGetValue(ev.Issuer, out BanInfo info))
            {
                info = new BanInfo();
                Plugin.BanInfo[ev.Issuer] = info;
            }

            if (info.BanCount >= Plugin.config.BanCount)
            {
                if (Plugin.config.UnBanPlayers)
                    info.UnbanAll();

                Webhook.Send(Plugin.config.WebHookText.ValidateText(ev.Issuer));

                ev.Issuer.Ban(Plugin.config.RaidReason, Plugin.config.RaiderBanDuration * 86400);
                ev.IsAllowed = false;

                return;
            }

            info.AddBan(ev.Player.UserId, ev.Player.IpAddress);
        }
    }
}
