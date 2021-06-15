using System;
namespace PlayerMatcher_RestAPI.Model
{
    public class User
    {

        public int id { get; set; }

        public bool status { get; set; } //ofline-online status

        public int level { get; set; } //player-level

        //public string character { get; set;  } //warriors??

        public string email { get; set; }

        public string password { get; set; }


        public User(int id, bool status, int level, string email, string password)
        {
            this.id = id;
            this.status = status;
            this.level = level;
            this.email = email;
            this.password = password;
        }
    }
}
