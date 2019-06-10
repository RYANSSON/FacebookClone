using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeFacebook
{
    [Serializable]
    public class UserOBJ
    
    {
        public String Email { get; set; }

        public String First { get; set; }

        public String Last { get; set; }

        public String ProfileURL { get; set; }
        public String Phone { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Organization { get; set; }

        public UserOBJ()

        {



        }

        public UserOBJ(String Email, String First, String Last, String ProfileURL,String Phone, String City, String State, String Organization)

        {

            this.Email = Email;

            this.First = First;

            this.Last = Last;

            this.ProfileURL = ProfileURL;
            this.Phone = Phone;
            this.City = City;

            this.State = State;

            this.Organization = Organization;

        }
    }
}
