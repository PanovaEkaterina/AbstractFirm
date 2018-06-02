using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPackageBlank.xaml
    /// </summary>
    public partial class FormPackageBlank : Window
    {
        public PackageBlankViewModel Model { set { model = value; } get { return model; } }

        private PackageBlankViewModel model;

        public FormPackageBlank()
        {
            InitializeComponent();
        }

        private void FormPackageBlank_Load(object sender, EventArgs e)
        {
            try
            {
                comboBoxBlank.DisplayMemberPath = "BlankName";
                comboBoxBlank.SelectedValuePath = "Id";
                comboBoxBlank.ItemsSource = Task.Run(() => APIKlient.GetRequestData<List<BlankViewModel>>("api/Blank/GetList")).Result;
                comboBoxBlank.SelectedItem = null;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxBlank.IsEnabled = false;
                comboBoxBlank.SelectedItem = model.BlankId;
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
