using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> SignUp(string email,string password,string username)
        {
            if(email== null || password == null || username == null || !CheckInputs(email,password,username))
            {
                return BadRequest();
            }
            var uuid = Guid.NewGuid();
            var account = new Account(uuid, email, password,username);
            var dbFeedback = DatabaseOperations.shared.SaveAccountToDB(account);

            if (dbFeedback == "true")
                return Ok(new { title = "Hesap basariyla olusturuldu"});
            else if(dbFeedback == "false")
                return Problem(title: "Girdiginiz bilgiler veri tabanında yer almaktadır, lutfen bilgilerinizi kontrol ediniz");
            else
                return Problem(title: "Sunucuda bir hata meydana geldi");

        }

        private bool CheckInputs(string email, string password, string username)
        {
            if (email.Equals("") || password.Equals("") || password.Length < 6 || username.Equals("") || email == null || password == null || username == null)
                return false;
            return true;
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> SignIn()
        {
            Account account = new Account(Guid.NewGuid(), "3123", "32131","312"); //dummy data
            if (DatabaseOperations.shared.CheckAccountFromDB(account))
            {
                return Ok(new { title = "Basari ile giris yaptiniz" });
            }
            else
            {
                return BadRequest(); //?
            }
        }


        [HttpGet("match")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<bool> MatchToPlayer()
        {
            return true;
        }

    }
}

