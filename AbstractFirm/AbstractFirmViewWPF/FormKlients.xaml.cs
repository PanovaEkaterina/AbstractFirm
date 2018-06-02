
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
    /// Логика взаимодействия для FormKlients.xaml
    /// </summary>
    public partial class FormKlients : Window
    {
        public FormKlients()
        {
            InitializeComponent();
            Loaded += FormKlients_Load;
        }

        private void FormKlients_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIKlient.GetRequest("api/Klient/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<KlientViewModel> list = APIKlient.GetElement<List<KlientViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewKlients.ItemsSource = list;
                        dataGridViewKlients.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewKlients.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormKlient();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewKlients.SelectedItem != null)
            {
                var form = new FormKlient();
                form.Id = ((KlientViewModel)dataGridViewKlients.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewKlients.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((KlientViewModel)dataGridViewKlients.SelectedItem).Id;
                    try
                    {
                        var response = APIKlient.PostRequest("api/Klient/DelElement", new KlientBindingModel { Id = id });
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
