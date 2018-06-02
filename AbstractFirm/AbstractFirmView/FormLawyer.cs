using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormLawyer : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormLawyer()
        {
            InitializeComponent();
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
                        textBoxFIO.Text = Lawyer.LawyerFIO;
                    }
                    else
                    {
                        throw new Exception(APIKlient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        LawyerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    response = APIKlient.PostRequest("api/Lawyer/AddElement", new LawyerBindingModel
                    {
                        LawyerFIO = textBoxFIO.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIKlient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
