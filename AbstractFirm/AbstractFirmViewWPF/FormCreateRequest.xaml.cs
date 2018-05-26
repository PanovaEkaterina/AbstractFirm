using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCreateRequest.xaml
    /// </summary>
    public partial class FormCreateRequest : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IKlientService serviceC;

        private readonly IPackageService serviceP;

        private readonly IMainService serviceM;

        public FormCreateRequest(IKlientService serviceC, IPackageService serviceP, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormCreateRequest_Load;
            comboBoxPackage.SelectionChanged += comboBoxPackage_SelectedIndexChanged;
            comboBoxPackage.SelectionChanged += new SelectionChangedEventHandler(comboBoxPackage_SelectedIndexChanged);
            this.serviceC = serviceC;
            this.serviceP = serviceP;
            this.serviceM = serviceM;
        }

        private void FormCreateRequest_Load(object sender, EventArgs e)
        {
            try
            {
                List<KlientViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxClient.DisplayMemberPath = "KlientFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listC;
                    comboBoxClient.SelectedItem = null;
                }
                List<PackageViewModel> listP = serviceP.GetList();
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
                    PackageViewModel product = serviceP.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product.Price).ToString();
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
                serviceM.CreateRequest(new RequestBindingModel
                {
                    KlientId = ((KlientViewModel)comboBoxClient.SelectedItem).Id,
                    PackageId = ((PackageViewModel)comboBoxPackage.SelectedItem).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
