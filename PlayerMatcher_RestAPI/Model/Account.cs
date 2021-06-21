using System;

namespace PlayerMatcher_RestAPI.Model
{
    public class Account
    {        
        public Guid id { get; set; } //Unique id-with same player's id 
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }

        public Account(Guid id, string email, string password,string username)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.username = username;
        }
    }
}
