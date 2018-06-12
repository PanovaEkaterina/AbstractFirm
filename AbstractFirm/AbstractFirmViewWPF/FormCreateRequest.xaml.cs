using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<KlientViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<KlientViewModel>>("api/Klient/GetList")).Result;
                if (list != null)
                {
                        comboBoxClient.DisplayMemberPath = "KlientFIO";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = list;
                        comboBoxClient.SelectedItem = null;
                }

                List<PackageViewModel> listP = Task.Run(() => APIKlient.GetRequestData<List<PackageViewModel>>("api/Package/GetList")).Result;
                if (listP != null)
                { 
                        comboBoxPackage.DisplayMemberPath = "PackageName";
                        comboBoxPackage.SelectedValuePath = "Id";
                        comboBoxPackage.ItemsSource = listP;
                        comboBoxPackage.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                    PackageViewModel product = Task.Run(() => APIKlient.GetRequestData<PackageViewModel>("api/Package/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)product.Price).ToString();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            int clientId = Convert.ToInt32(comboBoxClient.SelectedValue);
            int productId = Convert.ToInt32(comboBoxPackage.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIKlient.PostRequestData("api/Main/CreateRequest", new RequestBindingModel
            {
                KlientId = clientId,
                PackageId = productId,
                Count = count,
                Sum = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
