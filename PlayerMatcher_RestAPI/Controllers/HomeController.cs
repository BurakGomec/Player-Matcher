using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayerMatcher_RestAPI.Model;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Test() => DatabaseOperations.shared.SaveUserToDB();


        [HttpGet("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> SignUp(string email,string password,string username)
        {
            //Username,email,password incele ona göre devam et....
            if(email.Equals("") || password.Equals("")  || password.Length < 6 || username.Equals(""))           
                return BadRequest();

            var account = new Account(35, email, password);
            var dbFeedback = DatabaseOperations.shared.SignUp(account);

            if (dbFeedback)
                return Ok(new { title = "Hesap basarıyla olusturuldu" });
            else
                return Problem(title: "ID cakismasi"); 
        }

        private bool CheckInputs(string email, string password, string username)
        {
            bool result=true;

            //username farklı olmalı vs.

            return result;
        }
        /*
        [HttpGet]
        public string signIn()
        {
            return "I";
        }


        public string signUp()
        {
            return "U";
        }

        public User matchPlayer()
        {
            return new User(1, false, 12, "dsas", "231");
        }
        */

    }
}

