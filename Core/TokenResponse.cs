using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Core
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public int expiration { get; set; }
        public string type { get; set; }

    }
}