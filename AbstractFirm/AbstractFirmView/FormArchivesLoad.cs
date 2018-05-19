using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormArchivesLoad : Form
    {
        public FormArchivesLoad()
        {
            InitializeComponent();
        }

        private void FormArchivesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIKlient.GetRequest("api/Report/GetArchivesLoad");
                if (response.Result.IsSuccessStatusCode)
                {
                    dataGridView.Rows.Clear();
                    foreach (var elem in APIKlient.GetElement<List<ArchivesLoadViewModel>>(response))
                    {
                        dataGridView.Rows.Add(new object[] { elem.ArchiveName, "", "" });
                        foreach (var listElem in elem.Blanks)
                        {
                            dataGridView.Rows.Add(new object[] { "", listElem.BlankName, listElem.Count });
                        }
                        dataGridView.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(response));
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
                    var response = APIKlient.PostRequest("api/Report/SaveArchivesLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception(APIKlient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
