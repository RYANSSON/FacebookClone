using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeFacebook
{
    [Serializable]
    public class RequestOBJ
    {
        public String requestingEmail { get; set; }

        public String requestedEmail { get; set; }

        public String token { get; set; }

     


        public RequestOBJ()

        {



        }



        public RequestOBJ(String Login, String Photos, String Profile, String Personal)

        {

            this.requestingEmail = requestingEmail;

            this.requestedEmail = requestedEmail;

            this.token = token;

        }
    }
}
