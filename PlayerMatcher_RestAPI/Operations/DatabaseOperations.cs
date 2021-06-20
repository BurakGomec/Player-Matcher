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

        public bool CheckAccountFromDB(Account account) //Kullanıcıdan gelen giriş yap isteğindeki verileri veri tabanındaki veriler ile karşılaştırıp bool döönen metot
        {
            var db = client.GetDatabase("Store"); //MongoDB içerisinde yer alan "store" isimli veri tabanı alınıyor
            var collection = db.GetCollection<Account>("Accounts"); //MongoDB içerisinde yer alan "accounts" koleksiyonu alınıyor
            var allDocuments = collection.Find(new BsonDocument()).ToList(); //Koleksiyon içersinde yer alan tüm dökümanlar kullanılmak üzere list tipine çeviriliyor
          
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
                var collection = db.GetCollection<Account>("Accounts");
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


        public bool SavePlayerToDB(Account account) //Kullanıcı sisteme kayıt olurken, kullanıcının bilgileri ile varsayılan bir player hesabı oluşturup veri tabanına kayıt eden metot 
        {
            try
            {
                var db = client.GetDatabase("Store"); 
                var collection = db.GetCollection<Player>("Players");
                Player player = new Player(account.id,account.userName, true, 1,0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool DuplicatedDataControl(Account account) //Kullanıcıdan gelen kayıt ol isteğinde alınan e-posta, kullanıcı adı bilgilerin veri tabanında yer alıp almadığını kontrol eden metot
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
