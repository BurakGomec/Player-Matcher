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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Player> FindOpponent(string username)
        {
            if(ReferenceEquals(username, null))
            {
                return BadRequest();
            }

            Player player2 = FindSimilarPlayer(username);

            return Ok(player2);
        }

        //kNN with Euclidean Distance
        private Player FindSimilarPlayer(string username)
        {
            #region data importing
            Player player1 = DatabaseOperations.shared.FindPlayer(username);

            var allPlayers = DatabaseOperations.shared.GetAllPlayers();

            var ids = new List<Guid>();
            var levels = new List<double>();
            var kdRatious = new List<double>();

            foreach (var player in allPlayers)
            {
                ids.Add(player.id);
                levels.Add(player.level);
                kdRatious.Add(player.kdRatio);
            }
            #endregion

            #region normalizasyon
            //listelerin min-max değerleri alınır
            double minLevel = levels.Min(), maxLevel = levels.Max(), minKD = kdRatious.Min(), maxKD = kdRatious.Max();

            for (int i = 0; i < ids.Count; i++)
            {
                levels[i] = (levels[i] - minLevel) / (maxLevel - minLevel);
                kdRatious[i] = (kdRatious[i] - minKD) / (maxKD - minKD);
            }
            #endregion

            #region kNN
            //oyuncuların id leri ile öklid uzaklıkları hash lenir
            var euclideanDistances = new Dictionary<Guid, double>();

            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i] != player1.id)
                {
                    //öklid uzaklığı bulunur
                    double distance = Math.Sqrt(Math.Pow(player1.level - levels[i], 2) + Math.Pow(player1.kdRatio - kdRatious[i], 2));
                    var opponentCandidate = allPlayers.Find(x => x.id == ids[i]);

                    if(opponentCandidate.status)//rakip adayı online ise
                    {
                        euclideanDistances.Add(ids[i], distance);
                    }                   
                }
            }

            //uzaklıklar azalan bir şekilde sıralanırlar
            euclideanDistances = euclideanDistances.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, z => z.Value);

            //0. index de ki id ye sahip olan player2 bulunur
            Player player2 = allPlayers.Find(x => x.id == euclideanDistances.ElementAt(0).Key);
            #endregion

            return player2;
        }

        [HttpPatch("levelup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Player> LevelUp(string username)
        {
            Player player = DatabaseOperations.shared.FindPlayer(username);
            if (ReferenceEquals(player, null))
            {
                return BadRequest();
            }

            player.level += 1;

            bool feedback = DatabaseOperations.shared.UpdatePlayerStats(player);

            if(!feedback)
            {
                return Problem(title: "Oyuncu hesabi guncellenirken bir sorun olustu");
            }

            return Ok(player);
        }
    }
}
