using AbstractFirmService.ImplementationsList;
using AbstractFirmService.Interfaces;
using System;
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
