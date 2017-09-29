using Data.Dapper.Class;
using Data.Dapper.Interface;
using Microsoft.Practices.Unity;
using Service.Class;
using Service.Interface;
using System.Web.Http;
using Unity.WebApi;

namespace WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserRepository, UserRepository>();
        }
    }
}