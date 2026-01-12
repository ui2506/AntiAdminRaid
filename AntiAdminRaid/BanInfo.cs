using AntiAdminRaid.Events.Arguments;
using LabApi.Features.Wrappers;
using System.Collections.Generic;
using UnityEngine;
using static BanHandler;

namespace AntiAdminRaid
{
    public sealed class BanInfo
    {
        internal static readonly Dictionary<Player, BanInfo> Cache = new Dictionary<Player, BanInfo>();

        public Player Issuer { get; private set; }
        public List<string> BannedUserIds { get; private set; }
        public List<string> BannedIps { get; private set; }
        public List<float> BannedTime { get; private set; }

        internal ushort BanCount 
        { 
            get
            {
                ushort count = 0;

                for (ushort i = 0; i < BannedTime.Count; i++)
                {
                    if (BannedTime[i] - Time.time > 0)
                        count++;
                }

                return count;
            }
        }

        internal static void GetOrAdd(Player issuer, out BanInfo info)
        {
            if (Cache.TryGetValue(issuer, out info))
                return;

            info = new BanInfo
            {
                Issuer = issuer,
                BannedUserIds = new List<string>(),
                BannedIps = new List<string>(),
                BannedTime = new List<float>(),
            };
        }

        internal void AddBan(string userId, string ip)
        {
            BannedTime.Add(Time.time + Plugin.PLuginConfig.BanCountKD);

            if (BannedUserIds.Contains(userId))
                BannedUserIds.Add(userId);

            if (!BannedIps.Contains(ip))
                BannedIps.Add(ip);
        }

        internal void UnbanAll()
        {
            foreach (string item in BannedUserIds)
            {
                UnbanningEventArgs ev = new UnbanningEventArgs(Issuer, item);

                if (ev.IsAllowed)
                    RemoveBan(item, BanType.UserId);
            }

            foreach (string item in BannedIps)
            {
                UnbanningEventArgs ev = new UnbanningEventArgs(Issuer, item);

                if (ev.IsAllowed)
                    RemoveBan(item, BanType.IP);
            }
        }
    }
}
