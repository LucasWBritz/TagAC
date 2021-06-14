using System.Collections.Generic;

namespace TagAC.Domain.Commands
{
    public class QueryResponse<TData> : Response
    {
        public IEnumerable<TData> Data { get; set; } = new List<TData>();
    }
}
