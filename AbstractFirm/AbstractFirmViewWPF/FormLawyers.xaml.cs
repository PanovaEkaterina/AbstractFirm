using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<LawyerViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<LawyerViewModel>>("api/Lawyer/GetList")).Result;
                if (list != null)
                {
                        dataGridViewLawyers.ItemsSource = list;
                        dataGridViewLawyers.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewLawyers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormLawyer();
            form.ShowDialog();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewLawyers.SelectedItem != null)
            {
                var form = new FormLawyer
                {
                    Id = ((LawyerViewModel)dataGridViewLawyers.SelectedItem).Id
                };
                form.ShowDialog();
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
                    Task task = Task.Run(() => APIKlient.PostRequestData("api/Lawyer/DelElement", new KlientBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    task.ContinueWith((prevTask) =>
                    {
                        var ex = (Exception)prevTask.Exception;
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
