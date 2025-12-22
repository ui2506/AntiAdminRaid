using System.Collections.Generic;
using UnityEngine;
using static BanHandler;

namespace AntiAdminRaid
{
    internal class BanInfo
    {
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

        internal void UnbanAll()
        {
            foreach (string item in BannedUserIds)
                RemoveBan(item, BanType.UserId);

            foreach (string item in BannedIps)
                RemoveBan(item, BanType.UserId);
        }
    }
}
