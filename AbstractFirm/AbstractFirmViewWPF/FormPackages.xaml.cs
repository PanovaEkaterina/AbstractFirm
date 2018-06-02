using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPackages.xaml
    /// </summary>
    public partial class FormPackages : Window
    {
        public FormPackages()
        {
            InitializeComponent();
            Loaded += FormPackages_Load;
        }

        private void FormPackages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<PackageViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<PackageViewModel>>("api/Package/GetList")).Result;
                if (list != null)
                    {
                        dataGridViewPackages.ItemsSource = list;
                        dataGridViewPackages.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewPackages.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewPackages.Columns[3].Visibility = Visibility.Hidden;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormPackage();
            form.ShowDialog();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackages.SelectedItem != null)
            {
                var form = new FormPackage
                {
                    Id = ((PackageViewModel)dataGridViewPackages.SelectedItem).Id
                };
                form.ShowDialog();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackages.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((PackageViewModel)dataGridViewPackages.SelectedItem).Id;
                    Task task = Task.Run(() => APIKlient.PostRequestData("api/Package/DelElement", new KlientBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    task.ContinueWith((prevTask) =>
                    {
                        var ex = (Exception)prevTask.Exception;
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
