using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFAPI2
{
    [Serializable]
    public class PostOBJ
    {
        public String EmailWriter { get; set; }

        public String EmailOwner { get; set; }

        public String Content { get; set; }

        public DateTime Date { get; set; }


        public PostOBJ()

        {



        }



        public PostOBJ(String EmailWriter, String EmailOwner, String Content, DateTime Date)

        {

            this.EmailWriter = EmailWriter;

            this.EmailOwner = EmailOwner;

            this.Content = Content;

            this.Date = Date;


        }
    }
}
