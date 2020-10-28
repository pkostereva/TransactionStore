using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Options;

namespace TransactionStore.API.Secure
{
    public class CreatingIPWhiteList
    {
        private List<string> _authorizedIPAddresses;
        public CreatingIPWhiteList(IOptions<IPWhiteListConfiguration> ipWhiteList)
        {
            _authorizedIPAddresses = ipWhiteList.Value.AuthorizedIPAddresses;
        }
        public void GetWhiteIPs()
        {
            List<IPAddress> authorizedIPs = new List<IPAddress>();
            foreach (string ip in _authorizedIPAddresses)
            {
                authorizedIPs.Add(IPAddress.Parse(ip));
            }
            AllowedIPs.authorizedIPs = authorizedIPs;
        }
    }
}

