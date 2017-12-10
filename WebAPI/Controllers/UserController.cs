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

        [AllowAnonymous]
        [HttpGet]
        [Route("userlist")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Retorna Lista de usuários")]
        public IHttpActionResult GetusetList()
        {
            var ret = _userService.GetUserList();

            if (ret == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return Json(ret);
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

        /// <summary>
        /// Clear User Cache
        /// </summary>
        [AllowAnonymous]
        [HttpPut]
        [Route("clearcache")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(string), description: "Limpa o Cache")]
        public IHttpActionResult ClearCache()
        {
            _userService.ClearFullCache();
            return Ok();
        }
    }
}