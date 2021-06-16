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
        public string test() => DatabaseOperations.shared.saveUserToDB();


        [HttpGet("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string> SignUp(string email,string password,string username)
        {
            //Username,email,password incele ona göre devam et....
            if(email.Equals("") || password.Equals("")  || password.Length <= 6 || username.Equals(""))
            {
                return BadRequest();
            }
            //Username farklı olmalıdır
            Account account = new Account(10, email, password);

            var x = DatabaseOperations.shared.SignUp(account);
            return x;
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

