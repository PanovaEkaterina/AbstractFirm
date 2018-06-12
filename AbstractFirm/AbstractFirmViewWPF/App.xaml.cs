using AbstractFirmView;
using System;
using System.Windows;

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
            APIKlient.Connect();
            MailKlient.Connect();
            var application = new App();
            application.Run(new FormMain());
        }
    }
}
