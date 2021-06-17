using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerMatcher_RestAPI.Model
{
    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; } //ofline-online status
        public int level { get; set; } //player's level
        public double kdRatio { get; set; } //kill-death ratio
        //public string character { get; set;  } //warriors??
        public string email { get; set; }
        public string password { get; set; }

        //aynen evet ha doğru lan evet aynen kanka şey olmaz mı statusu online olanı bulcaz onun veritabanından ismini alıp geri döneriz ?
        //evet haa anladım :d olm harbi patlak o xd o zaman dur kanka requestten alalım aynen ? hatta yazsak ya şimdi aynen bi görelim ama hee
        //idler çakıştı duplicate aynen xdd 


        public Player(int id, bool status, int level, string email, string password)
        {
            this.id = id;
            this.status = status;
            this.level = level;
            this.email = email;
            this.password = password;
        }
    }
}
