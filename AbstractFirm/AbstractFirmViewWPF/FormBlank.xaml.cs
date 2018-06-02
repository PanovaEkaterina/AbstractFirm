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
    /// Логика взаимодействия для FormBlank.xaml
    /// </summary>
    public partial class FormBlank : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormBlank()
        {
            InitializeComponent();
            Loaded += FormBlank_Load;
        }

        private void FormBlank_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIKlient.GetRequest("api/Blank/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var blank = APIKlient.GetElement<BlankViewModel>(response);
                        textBoxName.Text = blank.BlankName;
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
                    response = APIKlient.PostRequest("api/Blank/UpdElement", new BlankBindingModel
                    {
                        Id = id.Value,
                        BlankName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIKlient.PostRequest("api/Blank/AddElement", new BlankBindingModel
                    {
                        BlankName = textBoxName.Text
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
