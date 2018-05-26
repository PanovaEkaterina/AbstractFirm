using AbstractFirmService;
using AbstractFirmService.ImplementationsBD;
using AbstractFirmService.Interfaces;
using System;
using System.Data.Entity;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<FormMain>());
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

            return currentContainer;
        }
    }
}
