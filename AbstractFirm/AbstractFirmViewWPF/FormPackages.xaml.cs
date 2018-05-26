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
    /// Логика взаимодействия для FormPackages.xaml
    /// </summary>
    public partial class FormPackages : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IPackageService service;

        public FormPackages(IPackageService service)
        {
            InitializeComponent();
            Loaded += FormPackages_Load;
            this.service = service;
        }

        private void FormPackages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<PackageViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewPackages.ItemsSource = list;
                    dataGridViewPackages.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPackages.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewPackages.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPackage>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackages.SelectedItem != null)
            {
                var form = Container.Resolve<FormPackage>();
                form.Id = ((PackageViewModel)dataGridViewPackages.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackages.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((PackageViewModel)dataGridViewPackages.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
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
    }
}
