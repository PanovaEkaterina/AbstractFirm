using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormArchive : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormArchive()
        {
            InitializeComponent();
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
                        dataGridView.DataSource = Archive.ArchiveBlanks;
                        dataGridView.Columns[0].Visible = false;
                        dataGridView.Columns[1].Visible = false;
                        dataGridView.Columns[2].Visible = false;
                        dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
