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
    /// Логика взаимодействия для FormTakeRequestInWork.xaml
    /// </summary>
    public partial class FormTakeRequestInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ILawyerService serviceI;

        private readonly IMainService serviceM;

        private int? id;

        public FormTakeRequestInWork(ILawyerService serviceI, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormTakeRequestInWork_Load;
            this.serviceI = serviceI;
            this.serviceM = serviceM;
        }

        private void FormTakeRequestInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указана заявка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<LawyerViewModel> listR = serviceI.GetList();
                if (listR != null)
                {
                    comboBoxLawyer.DisplayMemberPath = "LawyerFIO";
                    comboBoxLawyer.SelectedValuePath = "Id";
                    comboBoxLawyer.ItemsSource = listR;
                    comboBoxLawyer.SelectedItem = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxLawyer.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceM.TakeRequestInWork(new RequestBindingModel
                {
                    Id = id.Value,
                    LawyerId = ((LawyerViewModel)comboBoxLawyer.SelectedItem).Id,
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
