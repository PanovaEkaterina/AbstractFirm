using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    dataGridView.Items.Clear();
                    foreach (var elem in Task.Run(() => APIKlient.GetRequestData<List<ArchivesLoadViewModel>>("api/Report/GetArchivesLoad")).Result)
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Report/SaveArchivesLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
}
