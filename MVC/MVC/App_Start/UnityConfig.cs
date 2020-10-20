using ReadLater.Data;
using ReadLater.Services;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace MVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDbContext, ReadLaterDataContext>();
            container.RegisterType<ReadLater.Repository.IUnitOfWork, ReadLater.Repository.UnitOfWork>();
            container.RegisterType<IBookmarkService, BookmarkService>();
            container.RegisterType<ICategoryService, CategoryService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}