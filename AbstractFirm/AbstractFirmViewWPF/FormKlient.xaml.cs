using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormKlient.xaml
    /// </summary>
    public partial class FormKlient : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormKlient()
        {
            InitializeComponent();
            Loaded += FormKlient_Load;
        }

        private void FormKlient_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIKlient.GetRequest("api/Klient/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var klient = APIKlient.GetElement<KlientViewModel>(response);
                        textBoxFullName.Text = klient.KlientFIO;
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
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIKlient.PostRequest("api/Klient/UpdElement", new KlientBindingModel
                    {
                        Id = id.Value,
                        KlientFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    response = APIKlient.PostRequest("api/Klient/AddElement", new KlientBindingModel
                    {
                        KlientFIO = textBoxFullName.Text
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
