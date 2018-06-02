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
    /// Логика взаимодействия для FormArchives.xaml
    /// </summary>
    public partial class FormArchives : Window
    {
        public FormArchives()
        {
            InitializeComponent();
            Loaded += FormArchives_Load;
        }

        private void FormArchives_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ArchiveViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<ArchiveViewModel>>("api/Archive/GetList")).Result;
                if (list != null)
                    {
                        dataGridViewArchives.ItemsSource = list;
                        dataGridViewArchives.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewArchives.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormArchive();
            form.ShowDialog();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                var form = new FormArchive
                {
                    Id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id
                };
               
                form.ShowDialog();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id;
                    Task task = Task.Run(() => APIKlient.PostRequestData("api/Archive/DelElement", new KlientBindingModel { Id = id }));

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
