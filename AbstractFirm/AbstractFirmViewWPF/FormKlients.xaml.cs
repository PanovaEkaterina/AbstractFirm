﻿using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormKlients.xaml
    /// </summary>
    public partial class FormKlients : Window
    {
        public FormKlients()
        {
            InitializeComponent();
            Loaded += FormKlients_Load;
        }

        private void FormKlients_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                    List<KlientViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<KlientViewModel>>("api/Klient/GetList")).Result;
                if (list != null)
                    {
                        dataGridViewKlients.ItemsSource = list;
                        dataGridViewKlients.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewKlients.Columns[1].Width = DataGridLength.Auto;
                    }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormKlient();
            form.ShowDialog();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewKlients.SelectedItem != null)
            {
                var form = new FormKlient
                {
                    Id = ((KlientViewModel)dataGridViewKlients.SelectedItem).Id
                };
                form.ShowDialog();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewKlients.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((KlientViewModel)dataGridViewKlients.SelectedItem).Id;
                    Task task = Task.Run(() => APIKlient.PostRequestData("api/Klient/DelElement", new KlientBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    task.ContinueWith((prevTask) =>
                    {
                        var ex = (Exception)prevTask.Exception;
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
