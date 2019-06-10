using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Utilities;
namespace FakeFacebook
{
    [Serializable]
    public class SettingsOBJ
    {
        public String Login { get; set; }

        public String Photos { get; set; }

        public String Profile { get; set; }

        public String Personal { get; set; }


        public SettingsOBJ()

        {



        }



        public SettingsOBJ(String Login, String Photos, String Profile, String Personal)

        {

            this.Login = Login;

            this.Photos = Photos;

            this.Profile = Profile;

            this.Personal = Personal;


        }
    }
}
