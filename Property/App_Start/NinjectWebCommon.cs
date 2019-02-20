[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Property.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Property.NinjectWebCommon), "Stop")]

namespace Property
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Mvc;
    using Property.Service;
    using Quartz;
    using Quartz.Impl;
    using Web.AppStart;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);
                kernel.Bind<IScheduler>().ToMethod(x =>
                {
                    var sched = new StdSchedulerFactory().GetScheduler();
                    sched.JobFactory = new NinjectJobFactory(kernel);
                    return sched;
                });
                var scheduler = kernel.Get<IScheduler>();
                RegisterServices(kernel);
                DependencyResolver.SetResolver(new Property.NinjectMvcDependencyResolver(kernel));
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<IIdxResidentialService>().To<IdxResidentialService>();
            kernel.Bind<IVoxResidentialService>().To<VoxResidentialService>();
            kernel.Bind<IIdxCommercialService>().To<IdxCommercialService>();
            kernel.Bind<IVoxCommercialService>().To<VoxCommercialService>();
            kernel.Bind<IIdxCondoService>().To<IdxCondoService>();
            kernel.Bind<IVoxCondoService>().To<VoxCondoService>();
            kernel.Bind<IGenrateLatLongFromAddressServices>().To<GenrateLatLongFromAddressServices>();

        }        
    }
}
