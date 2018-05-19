using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnArchive.xaml
    /// </summary>
    public partial class FormPutOnArchive : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IArchiveService serviceS;

        private readonly IBlankService serviceC;

        private readonly IMainService serviceM;

        public FormPutOnArchive(IArchiveService serviceS, IBlankService serviceC, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormPutOnArchive_Load;
            this.serviceS = serviceS;
            this.serviceC = serviceC;
            this.serviceM = serviceM;
        }

        private void FormPutOnArchive_Load(object sender, EventArgs e)
        {
            try
            {
                List<BlankViewModel> listZ = serviceC.GetList();
                if (listZ != null)
                {
                    comboBoxBlank.DisplayMemberPath = "BlankName";
                    comboBoxBlank.SelectedValuePath = "Id";
                    comboBoxBlank.ItemsSource = listZ;
                    comboBoxBlank.SelectedItem = null;
                }
                List<ArchiveViewModel> listB = serviceS.GetList();
                if (listB != null)
                {
                    comboBoxArchive.DisplayMemberPath = "ArchiveName";
                    comboBoxArchive.SelectedValuePath = "Id";
                    comboBoxArchive.ItemsSource = listB;
                    comboBoxArchive.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxBlank.SelectedItem == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxArchive.SelectedItem == null)
            {
                MessageBox.Show("Выберите архив", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceM.PutBlankOnArchive(new ArchiveBlankBindingModel
                {
                    BlankId = Convert.ToInt32(comboBoxBlank.SelectedValue),
                    ArchiveId = Convert.ToInt32(comboBoxArchive.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }


    }
}
