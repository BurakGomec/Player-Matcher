using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlayerMatcher_RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string test()
        {
            return DatabaseOperations.shared.saveUserToDB();
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

