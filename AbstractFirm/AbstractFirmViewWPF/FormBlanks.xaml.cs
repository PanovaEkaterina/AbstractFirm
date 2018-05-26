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
    /// Логика взаимодействия для FormBlanks.xaml
    /// </summary>
    public partial class FormBlanks : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IBlankService service;

        public FormBlanks(IBlankService service)
        {
            InitializeComponent();
            Loaded += FormBlanks_Load;
            this.service = service;
        }

        private void FormBlanks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<BlankViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewBlanks.ItemsSource = list;
                    dataGridViewBlanks.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewBlanks.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormBlank>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlanks.SelectedItem != null)
            {
                var form = Container.Resolve<FormBlank>();
                form.Id = ((BlankViewModel)dataGridViewBlanks.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlanks.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((BlankViewModel)dataGridViewBlanks.SelectedItem).Id;
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
