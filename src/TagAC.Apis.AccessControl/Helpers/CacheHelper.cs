using System;
using TagAC.Apis.AccessControl.Sessions;

namespace TagAC.Apis.AccessControl.Helpers
{
    public static class CacheHelper
    {
        public static string ToCacheKey(this IHeaderParametersSession session)
        {
            return ToCacheKey(session.RFID, session.SmartLockId);
        } 
        public static Guid ToSmartLockId(this IHeaderParametersSession session)
        {
            return Guid.Parse(session.SmartLockId);
        }

        public static string ToCacheKey(string rfid, string deviceId)
        {
            return $"{rfid}:{deviceId}";
        }
    }
}
