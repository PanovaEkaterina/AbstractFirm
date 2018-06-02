using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using Microsoft.Win32;
using System;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormArchivesLoad.xaml
    /// </summary>
    public partial class FormArchivesLoad : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormArchivesLoad(IReportService service)
        {
            InitializeComponent();
            Loaded += FormArchivesLoad_Load;
            this.service = service;
        }

        private void FormArchivesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = service.GetArchivesLoad();
                if (dict != null)
                {
                    dataGridView.Items.Clear();
                    foreach (var elem in dict)
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
                    service.SaveArchivesLoad(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
