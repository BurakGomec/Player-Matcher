using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpGet()] //Test-Method..
        public ActionResult<string> Test()
        {
            return Ok(new { message = "Matched User Name : masomo" });
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> SignUp([FromBody] Account acc)
        {
            if (ReferenceEquals(acc.email, null) || ReferenceEquals(acc.password, null) || ReferenceEquals(acc.username, null) || acc.password.Length < 6)
                   return BadRequest();

            var uuid = Guid.NewGuid();
            var account = new Account(uuid, acc.email, acc.password, acc.username);
            var dbFeedback = DatabaseOperations.shared.SaveAccountToDB(account);

            if (dbFeedback == "true") 
            {
                if(DatabaseOperations.shared.SavePlayerToDB(account)) //Kullanıcı sisteme kayıt oldu ise kullanıcıya bir oyuncu hesabı oluşturulup veri tabanına kayıt ediliyor
                    return Ok(new { title = "Hesap basariyla olusturuldu" });
                else 
                    return Problem(title: "Hesabınız yaratılırken bir hata meydana geldi");
            }
            else if(dbFeedback == "false")
                return Problem(title: "Girdiginiz bilgiler veri tabanında yer almaktadır, lutfen bilgilerinizi kontrol ediniz");
            else
                return Problem(title: "Sunucuda bir hata meydana geldi");

        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> SignIn([FromBody] Account acc)
        {
            if (ReferenceEquals(acc.email, null) || ReferenceEquals(acc.password, null) || ReferenceEquals(acc.username, null) || acc.password.Length < 6)
                return BadRequest();

            Account account = new Account(Guid.NewGuid(), acc.email,acc.password,acc.username);
            if (DatabaseOperations.shared.CheckAccountFromDB(account))
            {
                return Ok(new { title = "Basari ile giris yaptiniz" });
            }
            else
            {
                return Problem(title: "Girdiginiz bilgiler hatalidir"); 
            }
        }

        [HttpGet("match")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> MatchToPlayer()
        {
            return true;
        }

        [HttpPut("update")] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> UpdateUserInfo([FromBody] Account acc, string userName)
        {
            if (ReferenceEquals(acc.email, null) || ReferenceEquals(acc.password, null) || ReferenceEquals(acc.username, null) || acc.password.Length < 6)
                return BadRequest();

            Account account = new Account(Guid.NewGuid(), acc.email, acc.password, acc.username);

            if (DatabaseOperations.shared.CheckAccountFromDB(account))
            {
                account.username = userName;
                if (DatabaseOperations.shared.UpdateUsername(account))
                {
                    return Ok( new { title = " Kullanıcı adı güncelleme işlemi başarılı" } );
                }
            }
            else
            {
                return Problem(title: "Girdiginiz bilgiler hatalidir");
            }

            return true;
        }
    }
}

