using System;
using MongoDB.Driver;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    public class DatabaseOperations
    {  
        public static DatabaseOperations shared = new DatabaseOperations();
        private MongoClient client = new MongoClient(Constant.Constants.connectionInfo);


        public string SaveUserToDB()
        {
            string result = "";

            try
            {
                //var client = new MongoClient(Constant.Constants.connectionInfo); //AYNEN GEREKTİ az önce 
                //var database = client.GetDatabase("Users");
                var db = client.GetDatabase("Store");

                var user = new Account(1, "sunny@gmail.com", "123456");

                var collection = db.GetCollection<Account>("Account");
                collection.InsertOne(user);

                var dbList = client.ListDatabases().ToList();

                foreach(var element in dbList)
                {
                    result += "   " +  element;
                }               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return result;
        }


        public bool SignUp(Account account)
        {
            try
            {
                var db = client.GetDatabase("Store");

                var collection = db.GetCollection<Account>("Account");

                collection.InsertOne(account);

                //Db'den id çekilecek ve +1 eklenerek save edilecek....
                
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }




        private DatabaseOperations(){}
       
    }
}
