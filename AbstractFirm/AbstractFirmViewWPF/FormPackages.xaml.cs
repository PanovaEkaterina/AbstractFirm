using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPackages.xaml
    /// </summary>
    public partial class FormPackages : Window
    {
        public FormPackages()
        {
            InitializeComponent();
            Loaded += FormPackages_Load;
        }

        private void FormPackages_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIKlient.GetRequest("api/Package/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<PackageViewModel> list = APIKlient.GetElement<List<PackageViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewPackages.ItemsSource = list;
                        dataGridViewPackages.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewPackages.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewPackages.Columns[3].Visibility = Visibility.Hidden;
                    }
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormPackage();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackages.SelectedItem != null)
            {
                var form = new FormPackage();
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
                        var response = APIKlient.PostRequest("api/Package/DelElement", new KlientBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIKlient.GetError(response));
                        }
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
