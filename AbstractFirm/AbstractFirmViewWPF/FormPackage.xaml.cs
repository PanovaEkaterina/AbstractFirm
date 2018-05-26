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
    /// Логика взаимодействия для FormPackage.xaml
    /// </summary>
    public partial class FormPackage : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IPackageService service;

        private int? id;

        private List<PackageBlankViewModel> packageBlanks;

        public FormPackage(IPackageService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += FormPackage_Load;
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    PackageViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.PackageName;
                        textBoxPrice.Text = view.Price.ToString();
                        packageBlanks = view.PackageBlanks;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                packageBlanks = new List<PackageBlankViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (packageBlanks != null)
                {
                    dataGridViewPackage.ItemsSource = null;
                    dataGridViewPackage.ItemsSource = packageBlanks;
                    dataGridViewPackage.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPackageBlank>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.PackageId = id.Value;
                    packageBlanks.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackage.SelectedItem != null)
            {
                var form = Container.Resolve<FormPackageBlank>();
                form.Model = packageBlanks[dataGridViewPackage.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    packageBlanks[dataGridViewPackage.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackage.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        packageBlanks.RemoveAt(dataGridViewPackage.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (packageBlanks == null || packageBlanks.Count == 0)
            {
                MessageBox.Show("Заполните бланки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<PackageBlankBindingModel> productComponentBM = new List<PackageBlankBindingModel>();
                for (int i = 0; i < packageBlanks.Count; ++i)
                {
                    productComponentBM.Add(new PackageBlankBindingModel
                    {
                        Id = packageBlanks[i].Id,
                        PackageId = packageBlanks[i].PackageId,
                        BlankId = packageBlanks[i].BlankId,
                        Count = packageBlanks[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new PackageBindingModel
                    {
                        Id = id.Value,
                        PackageName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        PackageBlanks = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new PackageBindingModel
                    {
                        PackageName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        PackageBlanks = productComponentBM
                    });
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
