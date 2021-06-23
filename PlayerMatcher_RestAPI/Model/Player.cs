using System;

namespace PlayerMatcher_RestAPI.Model
{
    //oyuncunun başka bir oyuncu ile eşleştirilmesi için gerekli bilgilerin tutulduğu sınıf
    public class Player
    {
        public Guid id { get; set; } //Unique id-with same account's id 
        public string username { get; set; } //Unique userName
        public bool status { get; set; } //Offline-online status
        public double level { get; set; } //Player's level
        public double kdRatio { get; set; } //Kill-death ratio

        public Player(Guid id, string username, bool status, int level, double kdRatio)
        {
            this.id = id;
            this.username = username;
            this.status = status;
            this.level = level;
            this.kdRatio = kdRatio;
        }
    }
}
