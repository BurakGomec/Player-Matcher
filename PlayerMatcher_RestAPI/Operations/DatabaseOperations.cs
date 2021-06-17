using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    public class DatabaseOperations
    {  
        public static DatabaseOperations shared = new DatabaseOperations();

        private MongoClient client = new MongoClient(Constant.Constants.connectionInfo);

        public bool CheckAccountFromDB(Account account)
        {

            var db = client.GetDatabase("Store");
            var collection = db.GetCollection<Account>("Account");
            var allDocuments = collection.Find(new BsonDocument()).ToList();

            foreach (var element in allDocuments)
            {
                if (element.email == account.email && element.userName == account.userName && element.password == account.password)
                {
                    return true;
                }
            }
            return false;
        }

        public string SaveAccountToDB(Account account)
        {
            try
            {
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Account>("Account");
                //var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
                if (DuplicatedDataControl(account))//Kullanıcıdan alınan bilgiler veritabanındaki bilgilerden eşsiz ise hesap kayıt işlemi yapılıyor
                {
                    collection.InsertOne(account);
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            catch(Exception)
            {
                return "error";
            }
        }


        private bool DuplicatedDataControl(Account account)
        {

            try
            {
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Account>("Account");
                var allDocuments = collection.Find(new BsonDocument()).ToList();
                foreach (var element in allDocuments)
                {
                    if(element.email == account.email || element.userName == account.userName)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private DatabaseOperations(){}
       
    }
}
