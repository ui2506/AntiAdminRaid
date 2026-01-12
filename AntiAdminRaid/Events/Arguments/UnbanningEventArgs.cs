using LabApi.Features.Wrappers;
using System;

namespace AntiAdminRaid.Events.Arguments
{
    public sealed class UnbanningEventArgs : EventArgs
    {
        public readonly Player Issuer;
        public readonly string TargetId;
        public bool IsAllowed;

        internal UnbanningEventArgs(Player issuer, string targetId)
        {
            Issuer = issuer;
            TargetId = targetId;
            IsAllowed = true;
        }
    }
}
