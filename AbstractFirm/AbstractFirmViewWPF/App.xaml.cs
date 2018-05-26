using AbstractFirmService.ImplementationsList;
using AbstractFirmService.Interfaces;
using System;
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
            currentContainer.RegisterType<IKlientService, KlientServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBlankService, BlankServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ILawyerService, LawyerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPackageService, PackageServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArchiveService, ArchiveServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
