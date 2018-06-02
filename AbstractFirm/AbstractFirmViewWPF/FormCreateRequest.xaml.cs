using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCreateRequest.xaml
    /// </summary>
    public partial class FormCreateRequest : Window
    {
        public FormCreateRequest()
        {
            InitializeComponent();
            Loaded += FormCreateRequest_Load;
            comboBoxPackage.SelectionChanged += comboBoxPackage_SelectedIndexChanged;
            comboBoxPackage.SelectionChanged += new SelectionChangedEventHandler(comboBoxPackage_SelectedIndexChanged);
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
                        comboBoxClient.DisplayMemberPath = "KlientFIO";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = list;
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
                        comboBoxPackage.DisplayMemberPath = "PackageName";
                        comboBoxPackage.SelectedValuePath = "Id";
                        comboBoxPackage.ItemsSource = list;
                        comboBoxPackage.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxPackage.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((PackageViewModel)comboBoxPackage.SelectedItem).Id;
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
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxPackage.SelectedItem == null)
            {
                MessageBox.Show("Выберите мебель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIKlient.PostRequest("api/Main/CreateRequest", new RequestBindingModel
                {
                    KlientId = Convert.ToInt32(comboBoxClient.SelectedValue),
                    PackageId = Convert.ToInt32(comboBoxPackage.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
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
