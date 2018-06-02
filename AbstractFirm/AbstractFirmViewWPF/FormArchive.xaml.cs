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
                    var response = APIKlient.GetRequest("api/Archive/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Archive = APIKlient.GetElement<ArchiveViewModel>(response);
                        textBoxName.Text = Archive.ArchiveName;
                        dataGridViewArchive.ItemsSource = Archive.ArchiveBlanks;
                        dataGridViewArchive.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewArchive.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewArchive.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewArchive.Columns[3].Width = DataGridLength.Auto;
                    }
                    else
                    {
                        throw new Exception(APIKlient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIKlient.PostRequest("api/Archive/UpdElement", new ArchiveBindingModel
                    {
                        Id = id.Value,
                        ArchiveName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIKlient.PostRequest("api/Archive/AddElement", new ArchiveBindingModel
                    {
                        ArchiveName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APIKlient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
