using System;

namespace SpaceTravel.Game
{
    public static class TimeUtil
    {
        public static long ToUnixSeconds(DateTime dtUtc)
            => (long)(dtUtc - DateTime.UnixEpoch).TotalSeconds;

        public static DateTime FromUnixSeconds(long unixSeconds)
            => DateTime.UnixEpoch.AddSeconds(unixSeconds);
    }
}