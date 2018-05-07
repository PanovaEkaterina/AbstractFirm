using AbstractFirmService;
using AbstractFirmService.ImplementationsBD;
using AbstractFirmService.Interfaces;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractFirmView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IKlientService, KlientServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBlankService, BlankServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ILawyerService, LawyerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPackageService, PackageServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArchiveService, ArchiveServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportServiceBD>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
