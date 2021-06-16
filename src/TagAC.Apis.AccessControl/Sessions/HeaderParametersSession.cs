namespace TagAC.Apis.AccessControl.Sessions
{
    public class HeaderParametersSession : IHeaderParametersSession
    {
        public string RFID { get; set; }
        public string SmartLockId { get; set; }       
    }
}
