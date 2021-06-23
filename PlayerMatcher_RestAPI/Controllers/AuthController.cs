using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayerMatcher_RestAPI.Model;
using System.Linq;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Dictionary<Guid, string> tokens = new Dictionary<Guid, string>();

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> SignUp([FromBody] Account acc)
        {
            if (ReferenceEquals(acc.email, null) || ReferenceEquals(acc.password, null) || ReferenceEquals(acc.username, null) || acc.password.Length < 6 || DatabaseOperations.shared.CheckEmail(acc.email))
                   return BadRequest();

            var uuid = Guid.NewGuid();
            var account = new Account(uuid, acc.email, acc.password, acc.username);
            string dbFeedback = DatabaseOperations.shared.SaveAccountToDB(account);

            if (dbFeedback.Equals("false")) 
                return Problem(title: "Girdiginiz bilgiler veri tabanında yer almaktadır, lutfen bilgilerinizi kontrol ediniz");                    

            if(dbFeedback.Equals("error"))
                return Problem(title: "Kullanici hesabiniz yaratılırken bir hata meydana geldi");

            //Kullanıcı sisteme kayıt oldu ise kullanıcıya bir oyuncu hesabı oluşturulup veri tabanına kayıt ediliyor
            bool feedback = DatabaseOperations.shared.SavePlayerToDB(account);

            if(!feedback)
            {
                return Problem(title: "Oyuncu hesabınız yaratılırken bir hata meydana geldi");
            }

            if (!tokens.ContainsKey(acc.id))
            {
                string token = DatabaseOperations.shared.Encypting(acc.username);
                tokens.Add(acc.id, token);
                return Ok(new { token = $"{token}" });
            }
            else
            {
                string token = tokens[acc.id];
                return Ok(new { token = $"{token}" });
            }
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> SignIn([FromBody] Account acc)
        {

            if (ReferenceEquals(acc.email, null) || ReferenceEquals(acc.password, null) || ReferenceEquals(acc.username, null) || acc.password.Length < 6 || DatabaseOperations.shared.CheckEmail(acc.email))
                return BadRequest();


            Account account = new Account(Guid.NewGuid(), acc.email,acc.password,acc.username);

            if (DatabaseOperations.shared.CheckAccountFromDB(account))
            {

                if (!tokens.ContainsKey(acc.id))
                {
                    string token = DatabaseOperations.shared.Encypting(acc.username);
                    tokens.Add(acc.id, token);
                    return Ok(new { token = $"{token}" });
                }
                else
                {
                    string token = tokens[acc.id];
                    return Ok(new { token = $"{token}" });
                }
            }
            else
            {
                return Problem(title: "Girdiginiz bilgiler hatalidir"); 
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> LogOut([FromBody] Dictionary<string, string> data)   
        {

            var username = data["username"];
            var token = data["token"];
            if (ReferenceEquals(username, null))
                return BadRequest();
  
            var playerDB = DatabaseOperations.shared.FindPlayer(username);

            if (ReferenceEquals(playerDB, null))
                return NotFound();

            if (!tokens.Any(x => x.Key == playerDB.id && x.Value == token))
                return Unauthorized();  

                playerDB.status = false;
            var control = DatabaseOperations.shared.UpdatePlayerStats(playerDB);

            if (!control)
                return Problem(title: "Çıkış yapılırken bir hata meydana geldi");

            //player'ın id si ile eşleşen Guid-string'i siler;
            tokens.Remove(playerDB.id);

            return Ok();
        }



        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> DeleteUser()
        {
            return true;
        }


    }
}

