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
    /// Логика взаимодействия для FormLawyer.xaml
    /// </summary>
    public partial class FormLawyer : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormLawyer()
        {
            InitializeComponent();
            Loaded += FormLawyer_Load;
        }

        private void FormLawyer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIKlient.GetRequest("api/Lawyer/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Lawyer = APIKlient.GetElement<LawyerViewModel>(response);
                        textBoxFullName.Text = Lawyer.LawyerFIO;
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
                    response = APIKlient.PostRequest("api/Lawyer/UpdElement", new LawyerBindingModel
                    {
                        Id = id.Value,
                        LawyerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    response = APIKlient.PostRequest("api/Lawyer/AddElement", new LawyerBindingModel
                    {
                        LawyerFIO = textBoxFullName.Text
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
