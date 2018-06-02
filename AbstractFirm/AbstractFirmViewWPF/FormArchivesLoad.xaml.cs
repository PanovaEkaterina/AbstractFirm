using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormArchivesLoad.xaml
    /// </summary>
    public partial class FormArchivesLoad : Window
    {
        public FormArchivesLoad()
        {
            InitializeComponent();
            Loaded += FormArchivesLoad_Load;
        }

        private void FormArchivesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIKlient.GetRequest("api/Report/GetArchivesLoad");
                if (response.Result.IsSuccessStatusCode)
                {                  
                    dataGridView.Items.Clear();
                    foreach (var elem in APIKlient.GetElement<List<ArchivesLoadViewModel>>(response))
                    {
                        dataGridView.Items.Add(new object[] { elem.ArchiveName, "", "" });
                        foreach (var listElem in elem.Blanks)
                        {
                            dataGridView.Items.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Items.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Items.Add(new object[] { });
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

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIKlient.PostRequest("api/Report/SaveArchivesLoad", new ReportBindingModel
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

    }
}
