using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormLawyers.xaml
    /// </summary>
    public partial class FormLawyers : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ILawyerService service;

        public FormLawyers(ILawyerService service)
        {
            InitializeComponent();
            Loaded += FormLawyers_Load;
            this.service = service;
        }

        private void FormLawyers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<LawyerViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewLawyers.ItemsSource = list;
                    dataGridViewLawyers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewLawyers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormLawyer>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewLawyers.SelectedItem != null)
            {
                var form = Container.Resolve<FormLawyer>();
                form.Id = ((LawyerViewModel)dataGridViewLawyers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewLawyers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((LawyerViewModel)dataGridViewLawyers.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
