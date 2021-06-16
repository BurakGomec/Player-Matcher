using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerMatcher_RestAPI.Model
{
    public class Account
    {        
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Account(int id, string email, string password)
        {
            this.id = id;
            this.email = email;
            this.password = password;
        }
    }
}
