namespace TagAC.Management.Domain.Queries.ListAccessControl
{
    public class ListAccessControlQuery : QueryCommand<ListAccessControlQueryResponse>
    {
        public string RFID { get; set; }
    }
}
