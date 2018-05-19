using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractFirmView
{
    public partial class FormPackage : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<PackageBlankViewModel> productComponents;

        public FormPackage()
        {
            InitializeComponent();
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var product = Task.Run(() => APIKlient.GetRequestData<PackageViewModel>("api/PackageProduct/Get/" + id.Value)).Result;
                    textBoxName.Text = product.PackageName;
                    textBoxPrice.Text = product.Price.ToString();
                    productComponents = product.PackageBlanks;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                productComponents = new List<PackageBlankViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (productComponents != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = productComponents;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormPackageBlank();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.PackageId = id.Value;
                    }
                    productComponents.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormPackageBlank();
                form.Model = productComponents[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productComponents[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        productComponents.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (productComponents == null || productComponents.Count == 0)
            {
                MessageBox.Show("Заполните бланки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<PackageBlankBindingModel> productComponentBM = new List<PackageBlankBindingModel>();
            for (int i = 0; i < productComponents.Count; ++i)
            {
                productComponentBM.Add(new PackageBlankBindingModel
                {
                    Id = productComponents[i].Id,
                    PackageId = productComponents[i].PackageId,
                    BlankId = productComponents[i].BlankId,
                    Count = productComponents[i].Count
                });
            }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Package/UpdElement", new PackageBindingModel
                {
                    Id = id.Value,
                    PackageName = name,
                    Price = price,
                    PackageBlanks = productComponentBM
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Package/AddElement", new PackageBindingModel
                {
                    PackageName = name,
                    Price = price,
                    PackageBlanks = productComponentBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
