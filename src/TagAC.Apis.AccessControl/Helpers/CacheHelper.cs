using TagAC.Apis.AccessControl.Sessions;

namespace TagAC.Apis.AccessControl.Helpers
{
    public static class CacheHelper
    {
        public static string ToCacheKey(this IHeaderParametersSession session)
        {
            return ToCacheKey(session.RFID, session.DeviceId.ToString());
        }

        public static string ToCacheKey(string rfid, string deviceId)
        {
            return $"{rfid}:{deviceId}";
        }
    }
}
