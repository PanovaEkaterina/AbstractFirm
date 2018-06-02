using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
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
                var responseC = APIKlient.GetRequest("api/Blank/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<BlankViewModel> list = APIKlient.GetElement<List<BlankViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxComponent.DisplayMember = "BlankName";
                        comboBoxComponent.ValueMember = "Id";
                        comboBoxComponent.DataSource = list;
                        comboBoxComponent.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseC));
                }
                var responseS = APIKlient.GetRequest("api/Archive/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<ArchiveViewModel> list = APIKlient.GetElement<List<ArchiveViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxStock.DisplayMember = "ArchiveName";
                        comboBoxStock.ValueMember = "Id";
                        comboBoxStock.DataSource = list;
                        comboBoxStock.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
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
                var response = APIKlient.PostRequest("api/Main/PutBlankOnArchive", new ArchiveBlankBindingModel
                {
                    BlankId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    ArchiveId = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
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
