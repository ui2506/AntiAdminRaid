using AntiAdminRaid.Events.Arguments;
using LabApi.Events;

namespace AntiAdminRaid.Events
{
    public static class Handlers
    {
        public static event LabEventHandler<UnbanningEventArgs> Unbanning;

        internal static void OnUnbanning(UnbanningEventArgs ev) => Unbanning?.InvokeEvent(ev);
    }
}
