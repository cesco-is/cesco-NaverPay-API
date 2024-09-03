using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaverPayAPI.Models
{
    public class RequestHeader
    {
        public RequestHeader(string clientId, string clientSecret, string chainId)
        {

            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.chainId = chainId;
        }
        public string clientId;
        public string clientSecret;
        public string chainId;
    }
}
