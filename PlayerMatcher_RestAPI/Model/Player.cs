using System;

namespace PlayerMatcher_RestAPI.Model
{
    public class Player
    {
        public Guid id { get; set; } //Unique id-with same account's id 
        public string userName { get; set; } //Unique userName
        public bool status { get; set; } //Offline-online status
        public int level { get; set; } //Player's level
        public double kdRatio { get; set; } //Kill-death ratio

        public Player(Guid id, string userName, bool status, int level, double kdRatio)
        {
            this.id = id;
            this.userName = userName;
            this.status = status;
            this.level = level;
            this.kdRatio = kdRatio;
        }
    }
}
