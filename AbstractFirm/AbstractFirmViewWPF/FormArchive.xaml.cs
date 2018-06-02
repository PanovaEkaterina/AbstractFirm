using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormArchive.xaml
    /// </summary>
    public partial class FormArchive : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormArchive()
        {
            InitializeComponent();
            Loaded += FormArchive_Load;
        }

        private void FormArchive_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var stock = Task.Run(() => APIKlient.GetRequestData<ArchiveViewModel>("api/Archive/Get/" + id.Value)).Result;
                    textBoxName.Text = stock.ArchiveName;
                    dataGridViewArchive.ItemsSource = stock.ArchiveBlanks;
                    dataGridViewArchive.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewArchive.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewArchive.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewArchive.Columns[3].Width = DataGridLength.Auto;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Archive/UpdElement", new ArchiveBindingModel
                {
                    Id = id.Value,
                    ArchiveName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Archive/AddElement", new ArchiveBindingModel
                {
                    ArchiveName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
