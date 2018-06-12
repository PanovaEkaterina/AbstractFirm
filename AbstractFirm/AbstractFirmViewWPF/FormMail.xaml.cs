using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormMail.xaml
    /// </summary>
    public partial class FormMail : Window
    {
        public FormMail()
        {
            InitializeComponent();
            Loaded += FormMail_Load;
        }
        private void FormMail_Load(object sender, EventArgs e)
        {
            try
            {
                List<MessageInfoViewModel> list = Task.Run(() =>
                APIKlient.GetRequestData<List<MessageInfoViewModel>>("api/MessageInfo/GetList")).Result;
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
