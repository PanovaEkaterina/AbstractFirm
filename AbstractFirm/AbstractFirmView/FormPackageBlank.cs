using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractFirmView
{
    public partial class FormPackageBlank : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public PackageBlankViewModel Model { set { model = value; } get { return model; } }

        private readonly IBlankService service;

        private PackageBlankViewModel model;

        public FormPackageBlank(IBlankService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormProductComponent_Load(object sender, EventArgs e)
        {
            try
            {
                List<BlankViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBoxComponent.DisplayMember = "BlankName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = list;
                    comboBoxComponent.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxComponent.Enabled = false;
                comboBoxComponent.SelectedValue = model.BlankId;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new PackageBlankViewModel
                    {
                        BlankId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                        BlankName = comboBoxComponent.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
