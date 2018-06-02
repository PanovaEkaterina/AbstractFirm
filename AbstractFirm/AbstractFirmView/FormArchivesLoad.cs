using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractFirmView
{
    public partial class FormArchivesLoad : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormArchivesLoad(IReportService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormArchivesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = service.GetArchivesLoad();
                if (dict != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridView.Rows.Add(new object[] { elem.ArchiveName, "", "" });
                        foreach (var listElem in elem.Blanks)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    service.SaveArchivesLoad(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
