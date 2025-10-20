namespace AntiAdminRaid.EventHandlers
{
    internal class ServerEvents
    {
        internal void Register() => LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
        
        internal void Unregister() => LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;

        private void OnRestartingRound() => Plugin.BanInfo.Clear();
    }
}
