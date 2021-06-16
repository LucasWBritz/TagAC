namespace TagAC.Apis.AccessControl.Sessions
{
    public interface IHeaderParametersSession
    {
        string RFID { get; set; }
        string SmartLockId { get; set; }       
    }
}
