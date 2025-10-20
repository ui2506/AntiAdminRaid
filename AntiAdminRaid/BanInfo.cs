using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BanHandler;

namespace AntiAdminRaid
{
    internal class BanInfo
    {
        internal int BanCount { 
            get
            {
                return BannedTime.Count(x => x - Time.time > 0);
            }
        }
        internal List<string> BannedUserIds { get; private set; }
        internal List<string> BannedIps { get; private set; }
        private List<float> BannedTime { get; set; }

        internal BanInfo()
        {
            BannedUserIds = new List<string>();
            BannedIps = new List<string>();
            BannedTime = new List<float>();
        }

        internal void AddBan(string userId, string ip)
        {
            BannedTime.Add(Time.time + Plugin.config.BanCountKD);

            if (BannedUserIds.Contains(userId))
                BannedUserIds.Add(userId);

            if (!BannedIps.Contains(ip))
                BannedIps.Add(ip);
        }

        internal void AddBan(string targetId)
        {
            BannedTime.Add(Time.time + Plugin.config.BanCountKD);

            bool isIp = Misc.ValidateIpOrHostname(targetId, out Misc.IPAddressType _, false, false);

            if (isIp)
            {
                if (!BannedIps.Contains(targetId))
                    BannedIps.Add(targetId);
            }
            else
            {
                if (BannedUserIds.Contains(targetId))
                    BannedUserIds.Add(targetId);
            }
        }

        internal void UnbanAll()
        {
            foreach (string item in BannedUserIds)
                RemoveBan(item, BanType.UserId);

            foreach (string item in BannedIps)
                RemoveBan(item, BanType.UserId);
        }
    }
}
