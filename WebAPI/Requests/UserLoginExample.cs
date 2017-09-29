using WebAPI.Filters;

namespace WebAPI.Requests
{
    public class UserLoginExample : MultipartRequestOperationFilter.IExamplesProvider
    {
        public object GetExamples()
        {
            return new UserLogin()
            {
                UserName = "username",
                Password = "password"
            };
        }
    }
}