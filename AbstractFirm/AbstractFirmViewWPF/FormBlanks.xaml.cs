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
    /// Логика взаимодействия для FormBlanks.xaml
    /// </summary>
    public partial class FormBlanks : Window
    {
        public FormBlanks()
        {
            InitializeComponent();
            Loaded += FormBlanks_Load;
        }

        private void FormBlanks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIKlient.GetRequest("api/Blank/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<BlankViewModel> list = APIKlient.GetElement<List<BlankViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewBlanks.ItemsSource = list;
                        dataGridViewBlanks.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewBlanks.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormBlank();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlanks.SelectedItem != null)
            {
                var form = new FormBlank();
                form.Id = ((BlankViewModel)dataGridViewBlanks.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlanks.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((BlankViewModel)dataGridViewBlanks.SelectedItem).Id;
                    try
                    {
                        var response = APIKlient.PostRequest("api/Blank/DelElement", new KlientBindingModel { Id = id });
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
