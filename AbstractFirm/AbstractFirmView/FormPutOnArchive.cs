using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormPutOnArchive : Form
    {
        public FormPutOnArchive()
        {
            InitializeComponent();
        }

        private void FormPutOnArchive_Load(object sender, EventArgs e)
        {
            try
            {
                List<BlankViewModel> listC = Task.Run(() => APIKlient.GetRequestData<List<BlankViewModel>>("api/Blank/GetList")).Result;
                if (listC != null)
                {
                    comboBoxComponent.DisplayMember = "BlankName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = listC;
                    comboBoxComponent.SelectedItem = null;
                }

                List<ArchiveViewModel> listS = Task.Run(() => APIKlient.GetRequestData<List<ArchiveViewModel>>("api/Archive/GetList")).Result;
                if (listS != null)
                {
                    comboBoxStock.DisplayMember = "ArchiveName";
                    comboBoxStock.ValueMember = "Id";
                    comboBoxStock.DataSource = listS;
                    comboBoxStock.SelectedItem = null;
                }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStock.SelectedValue == null)
            {
                MessageBox.Show("Выберите архив", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int componentId = Convert.ToInt32(comboBoxComponent.SelectedValue);
                int stockId = Convert.ToInt32(comboBoxStock.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Main/PutBlankOnArchive", new ArchiveBlankBindingModel
                {
                    BlankId = componentId,
                    ArchiveId = stockId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Архив пополнен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
