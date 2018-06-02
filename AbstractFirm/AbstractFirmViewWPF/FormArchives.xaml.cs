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
    /// Логика взаимодействия для FormArchives.xaml
    /// </summary>
    public partial class FormArchives : Window
    {
        public FormArchives()
        {
            InitializeComponent();
            Loaded += FormArchives_Load;
        }

        private void FormArchives_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIKlient.GetRequest("api/Archive/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ArchiveViewModel> list = APIKlient.GetElement<List<ArchiveViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewArchives.ItemsSource = list;
                        dataGridViewArchives.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewArchives.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormArchive();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                var form = new FormArchive();
                form.Id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id;
                    try
                    {
                        var response = APIKlient.PostRequest("api/Archive/DelElement", new KlientBindingModel { Id = id });
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
