using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormCreateRequest : Form
    {
        public FormCreateRequest()
        {
            InitializeComponent();
        }

        private void FormCreateRequest_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIKlient.GetRequest("api/Klient/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<KlientViewModel> list = APIKlient.GetElement<List<KlientViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxClient.DisplayMember = "KlientFIO";
                        comboBoxClient.ValueMember = "Id";
                        comboBoxClient.DataSource = list;
                        comboBoxClient.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseC));
                }
                var responseP = APIKlient.GetRequest("api/Package/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<PackageViewModel> list = APIKlient.GetElement<List<PackageViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxProduct.DisplayMember = "PackageName";
                        comboBoxProduct.ValueMember = "Id";
                        comboBoxProduct.DataSource = list;
                        comboBoxProduct.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxProduct.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxProduct.SelectedValue);
                    var responseP = APIKlient.GetRequest("api/Product/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        PackageViewModel product = APIKlient.GetElement<PackageViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)product.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIKlient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxClient.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите пакет документов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIKlient.PostRequest("api/Main/CreateRequest", new RequestBindingModel
                {
                    KlientId = Convert.ToInt32(comboBoxClient.SelectedValue),
                    PackageId = Convert.ToInt32(comboBoxProduct.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
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
