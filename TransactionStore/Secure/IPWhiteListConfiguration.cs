using System.Collections.Generic;

namespace TransactionStore.API.Secure
{
    public class IPWhiteListConfiguration
    {
        public List<string> AuthorizedIPAddresses { get; set; }
    }
}
