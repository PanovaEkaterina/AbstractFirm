using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                var response = APIKlient.GetRequest("api/Main/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<RequestViewModel> list = APIKlient.GetElement<List<RequestViewModel>>(response);
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
                try
                {
                    var response = APIKlient.PostRequest("api/Main/FinishRequest", new RequestBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        private void buttonPayRequest_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    var response = APIKlient.PostRequest("api/Main/.PayRequest", new RequestBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
                try
                {
                    var response = APIKlient.PostRequest("api/Report/SavePackagePrice", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
