using TagAC.Apis.AccessControl.Sessions;

namespace TagAC.Apis.AccessControl.Helpers
{
    public static class CacheHelper
    {
        public static string ToCacheKey(this IHeaderParametersSession session)
        {
            return $"{session.RFID}:{session.DeviceId}";
        }
    }
}
