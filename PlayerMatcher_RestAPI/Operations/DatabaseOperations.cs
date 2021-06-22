using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    public class DatabaseOperations
    {  
        public static DatabaseOperations shared = new DatabaseOperations();

        public MongoClient client = new MongoClient(Constant.Constants.connectionInfo);

        public bool CheckAccountFromDB(Account account) //Kullanıcıdan gelen giriş yap isteğindeki verileri veri tabanındaki veriler ile karşılaştırıp bool döönen metot
        {
            try
            {
                var db = client.GetDatabase("Store"); //MongoDB içerisinde yer alan "store" isimli veri tabanı alınıyor
                var collection = db.GetCollection<Account>("Accounts"); //MongoDB içerisinde yer alan "accounts" koleksiyonu alınıyor
                var allDocuments = collection.Find(new BsonDocument()).ToList(); //Koleksiyon içersinde yer alan tüm dökümanlar kullanılmak üzere list tipine çeviriliyor
                var encryptedPassword = EncryptingPassword(account.password);

                foreach (var element in allDocuments)
                {
                    if (element.email == account.email && element.username == account.username && element.password == encryptedPassword)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public string SaveAccountToDB(Account account)
        {
            try
            {
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Account>("Accounts");
                var password = EncryptingPassword(account.password);
                account.password = password;

                if (DuplicatedDataControl(account))//Kullanıcıdan alınan bilgiler veritabanındaki bilgilerden eşsiz ise hesap kayıt işlemi yapılıyor
                {
                    account.password = EncryptingPassword(account.password);
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
                Player player = new Player(account.id,account.username, true, 1,0);
                collection.InsertOne(player);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UpdateUsername(Account account)
        {
            try
            {
                /*
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Player>("Players");
                collection.UpdateOne(account);
                */

                return true;
            }
            catch
            {
                return false;
            }
        }

        //player sınıfının güncelleştirilebilen tüm alanları için tek bir metod;
        public bool UpdatePlayerStats(Player player)
        {
            try
            {
 
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Player>("Players");

                var filter = Builders<Player>.Filter.Eq(x => x.id, player.id);
    
                var update = Builders<Player>.Update.Set(x => x.level, player.level);

                collection.FindOneAndUpdate(filter, update);
   
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString()); 
                return false;
            }
        }

        public Player FindPlayer(string username)
        {
            var db = client.GetDatabase("Store");
            var collection = db.GetCollection<Player>("Players");
            List<Player> players = collection.Find(new BsonDocument()).ToList();
   
            Player player = players.Find(x => x.username == username);
            return player;
        }

        public List<Player> GetAllPlayers()
        {
            var db = client.GetDatabase("Store");
            var collection = db.GetCollection<Player>("Players");

            List<Player> players = collection.Find(new BsonDocument()).ToList();

            return players;
        }

        private bool DuplicatedDataControl(Account account) //Kullanıcıdan gelen kayıt ol isteğinde alınan e-posta, kullanıcı adı bilgilerin veri tabanında yer alıp almadığını kontrol eden metot
        {
            try
            {
                var db = client.GetDatabase("Store");
                var collection = db.GetCollection<Account>("Accounts");
                var allDocuments = collection.Find(new BsonDocument()).ToList();
                foreach (var element in allDocuments)
                {
                    if(element.email == account.email || element.username == account.username)
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

        //Kullanıcının girdiği parolanın veritabanına kaydedilmeden önce şifrelendiği metot
        private string EncryptingPassword(string password)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


        private DatabaseOperations(){}
       
    }
}
