using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayerMatcher_RestAPI.Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GameSceneController : ControllerBase
    {
        //gönderilen oyuncuya en benzeyen oyuncuyu geri döndüren metod 
        [HttpGet("matchmaking")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Player> FindOpponent([FromBody] Player player1)
        {
            if(ReferenceEquals(player1, null))
            {
                return BadRequest();
            }

            Player player2 = FindSimilarPlayer(player1);

            return Ok(player2);
        }

        //kNN with Euclidean Distance
        private Player FindSimilarPlayer(Player player1)
        {
            #region data importing
            var db = DatabaseOperations.shared.client.GetDatabase("Store");
            var players = db.GetCollection<Player>("Players");

            //var filter = Builders<Player>.Filter.Empty;
            //var playerList = players.Find(filter).ToList();

            var allDocuments = players.Find(new BsonDocument()).ToList();

            var levels = new List<double>();
            var kdRatious = new List<double>();
            #endregion

            #region normalizasyon
            foreach (var player in allDocuments)
            {
                levels.Add(player.level);
                kdRatious.Add(player.kdRatio);
            }

            //listelerin min-max değerleri alınır
            double minLevel = levels.Min(), maxLevel = levels.Max(), minKD = kdRatious.Min(), maxKD = kdRatious.Max();

            foreach (var player in allDocuments)
            {
                player.level = (player.level - minLevel) / (maxLevel-minLevel);
                player.kdRatio = (player.kdRatio - minKD) / (maxKD - minKD); 
            }
            #endregion

            #region kNN
            //oyuncuların id leri ile öklid uzaklıkları hash lenir
            var euclideanDistances = new Dictionary<Guid, double>();

            foreach (var player in allDocuments)
            {
                if(player.id != player1.id)
                {
                    //öklid uzaklığı bulunur
                    double distance = Math.Sqrt(Math.Pow(player1.level - player.level, 2) + Math.Pow(player1.kdRatio - player.kdRatio, 2));
                    euclideanDistances.Add(player.id, distance);
                }                
            }

            //uzaklıklar azalan bir şekilde sıralanırlar
            euclideanDistances = euclideanDistances.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, z => z.Value);

            //0. index de ki id ye sahip olan player2 bulunur
            Player player2 = allDocuments.Find(x => x.id == euclideanDistances.ElementAt(0).Key);
            #endregion

            return player2;
        }
    }
}
