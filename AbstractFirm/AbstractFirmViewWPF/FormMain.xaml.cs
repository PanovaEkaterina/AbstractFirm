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
    /// Логика взаимодействия для FormMain.xaml
    /// </summary>
    public partial class FormMain : Window
    {

        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IMainService service;

        public FormMain(IMainService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void LoadData()
        {
            try
            {
                List<RequestViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewMain.ItemsSource = list;
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormKlients>();
            form.ShowDialog();
        }

        private void бланкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormBlanks>();
            form.ShowDialog();
        }

        private void пакетдокументовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPackages>();
            form.ShowDialog();
        }

        private void архивыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormArchives>();
            form.ShowDialog();
        }

        private void юристыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormLawyers>();
            form.ShowDialog();
        }

        private void пополнитьАрхивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPutOnArchive>();
            form.ShowDialog();
        }

        private void buttonCreateRequest_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateRequest>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeRequestInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = Container.Resolve<FormTakeRequestInWork>();
                form.Id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonRequestReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    service.FinishRequest(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayRequest_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((RequestViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    service.PayRequest(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
