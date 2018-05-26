using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPackageBlank.xaml
    /// </summary>
    public partial class FormPackageBlank : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public PackageBlankViewModel Model { set { model = value; } get { return model; } }

        private readonly IBlankService service;

        private PackageBlankViewModel model;

        public FormPackageBlank(IBlankService service)
        {
            InitializeComponent();
            Loaded += FormPackageBlank_Load;
            this.service = service;
        }

        private void FormPackageBlank_Load(object sender, EventArgs e)
        {
            List<BlankViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxBlank.DisplayMemberPath = "BlankName";
                    comboBoxBlank.SelectedValuePath = "Id";
                    comboBoxBlank.ItemsSource = list;
                    comboBoxBlank.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxBlank.IsEnabled = false;
                foreach (BlankViewModel item in list)
                {
                    if (item.BlankName == model.BlankName)
                    {
                        comboBoxBlank.SelectedItem = item;
                    }
                }
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxBlank.SelectedItem == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new PackageBlankViewModel
                    {
                        BlankId = Convert.ToInt32(comboBoxBlank.SelectedValue),
                        BlankName = comboBoxBlank.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
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
