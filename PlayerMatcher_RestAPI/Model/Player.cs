using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerMatcher_RestAPI.Model
{
    public class Player
    {
        public Guid id { get; set; } //unique id 
        public string userName { get; set; }
        public bool status { get; set; } //ofline-online status
        public int level { get; set; } //player's level
        public double kdRatio { get; set; } //kill-death ratio
        public string email { get; set; }
        public string password { get; set; }


        public Player(Guid id, bool status, int level, string email, string password)
        {
            this.id = id;
            this.status = status;
            this.level = level;
            this.email = email;
            this.password = password;
        }
    }
}
