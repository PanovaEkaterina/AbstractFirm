using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
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
                    var stock = Task.Run(() => APIKlient.GetRequestData<ArchiveViewModel>("api/Archive/Get/" + id.Value)).Result;
                    textBoxName.Text = stock.ArchiveName;
                    dataGridView.DataSource = stock.ArchiveBlanks;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
