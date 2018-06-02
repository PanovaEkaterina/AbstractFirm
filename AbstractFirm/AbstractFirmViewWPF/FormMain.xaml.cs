using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormMain.xaml
    /// </summary>
    public partial class FormMain : Window
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<RequestViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<RequestViewModel>>("api/Main/GetList")).Result;
                if (list != null)
                 {
                        dataGridViewMain.ItemsSource = list;
                        dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                        dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
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

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormKlients();
            form.ShowDialog();
        }

        private void бланкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormBlanks();
            form.ShowDialog();
        }

        private void пакетдокументовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPackages();
            form.ShowDialog();
        }

        private void архивыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormArchives();
            form.ShowDialog();
        }

        private void юристыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormLawyers();
            form.ShowDialog();
        }

        private void пополнитьАрхивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPutOnArchive();
            form.ShowDialog();
        }

        private void buttonCreateRequest_Click(object sender, EventArgs e)
        {
            var form = new FormCreateRequest();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeRequestInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = new FormTakeRequestInWork
                {
                    Id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id
                };
                              
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonRequestReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Main/FinishRequest", new RequestBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonPayRequest_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Main/PayRequest", new RequestBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсИзделийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Report/SavePackagePrice", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void загруженностьСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormArchivesLoad();
            form.ShowDialog();
        }

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormKlientRequests();
            form.ShowDialog();
        }
    }
}
