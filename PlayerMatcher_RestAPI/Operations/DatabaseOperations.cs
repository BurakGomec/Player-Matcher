using System;
using MongoDB.Driver;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    public class DatabaseOperations
    {  
        public static DatabaseOperations shared = new DatabaseOperations();

        public string saveUserToDB()
        {
            string result = "";

            try
            {
                var client = new MongoClient(Constant.Constants.connectionInfo);
                //var database = client.GetDatabase("Users");
                var db = client.GetDatabase("Store");

                var user = new Account(1, "sunny@gmail.com", "123456");

                var collection = db.GetCollection<Account>("Users");
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

        private DatabaseOperations(){}
       
    }
}
