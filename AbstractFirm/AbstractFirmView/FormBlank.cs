using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormBlank : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormBlank()
        {
            InitializeComponent();
        }

        private void FormBlank_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var component = Task.Run(() => APIKlient.GetRequestData<BlankViewModel>("api/Blank/Get/" + id.Value)).Result;
                    textBoxName.Text = component.BlankName;
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
                task = Task.Run(() => APIKlient.PostRequestData("api/Blank/UpdElement", new BlankBindingModel
                {
                    Id = id.Value,
                    BlankName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Blank/AddElement", new BlankBindingModel
                {
                    BlankName = name
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
