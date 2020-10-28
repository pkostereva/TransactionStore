using System.Collections.Generic;
using System.Net;

namespace TransactionStore.API.Secure
{
    public static class AllowedIPs
    {
        public static List<IPAddress> authorizedIPs { get; set; }
    }
}
