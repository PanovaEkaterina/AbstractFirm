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
    /// Логика взаимодействия для FormArchives.xaml
    /// </summary>
    public partial class FormArchives : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IArchiveService service;

        public FormArchives(IArchiveService service)
        {
            InitializeComponent();
            Loaded += FormArchives_Load;
            this.service = service;
        }

        private void FormArchives_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ArchiveViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewArchives.ItemsSource = list;
                    dataGridViewArchives.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewArchives.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormArchive>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                var form = Container.Resolve<FormArchive>();
                form.Id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewArchives.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ArchiveViewModel)dataGridViewArchives.SelectedItem).Id;
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
