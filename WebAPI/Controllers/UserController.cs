using Service.Interface;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Requests;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        protected readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Generate new Token / User Validate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Gera Token")]
        public IHttpActionResult ValidateUser([FromBody]UserLogin request)
        {
            var ret = _userService.GetToken(request.UserName, request.Password);

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
        }

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmpassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Insere Usuario")]
        public IHttpActionResult InsertUser(string username, string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                _userService.InsertUser(username, password);
                return Json("User Created Successfully! :)");
            }
            throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
        }
    }
}