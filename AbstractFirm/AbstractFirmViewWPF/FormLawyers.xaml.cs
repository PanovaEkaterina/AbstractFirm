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
    /// Логика взаимодействия для FormLawyers.xaml
    /// </summary>
    public partial class FormLawyers : Window
    {
        public FormLawyers()
        {
            InitializeComponent();
            Loaded += FormLawyers_Load;
        }

        private void FormLawyers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIKlient.GetRequest("api/Lawyer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<LawyerViewModel> list = APIKlient.GetElement<List<LawyerViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewLawyers.ItemsSource = list;
                        dataGridViewLawyers.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewLawyers.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormLawyer();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewLawyers.SelectedItem != null)
            {
                var form = new FormLawyer();
                form.Id = ((LawyerViewModel)dataGridViewLawyers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewLawyers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((LawyerViewModel)dataGridViewLawyers.SelectedItem).Id;
                    try
                    {
                        var response = APIKlient.PostRequest("api/Lawyer/DelElement", new KlientBindingModel { Id = id });
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
