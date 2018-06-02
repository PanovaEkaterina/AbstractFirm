using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnArchive.xaml
    /// </summary>
    public partial class FormPutOnArchive : Window
    {
        public FormPutOnArchive()
        {
            InitializeComponent();
            Loaded += FormPutOnArchive_Load;
        }

        private void FormPutOnArchive_Load(object sender, EventArgs e)
        {
            try
            {
                List<BlankViewModel> list = Task.Run(() => APIKlient.GetRequestData<List<BlankViewModel>>("api/Blank/GetList")).Result;
                if (list != null)
                    {
                        comboBoxBlank.DisplayMemberPath = "BlankName";
                        comboBoxBlank.SelectedValuePath = "Id";
                        comboBoxBlank.ItemsSource = list;
                        comboBoxBlank.SelectedItem = null;
                    }
                    List<ArchiveViewModel> listA = Task.Run(() => APIKlient.GetRequestData<List<ArchiveViewModel>>("api/Archive/GetList")).Result;
                if (listA != null)
                    {
                        comboBoxArchive.DisplayMemberPath = "ArchiveName";
                        comboBoxArchive.SelectedValuePath = "Id";
                        comboBoxArchive.ItemsSource = list;
                        comboBoxArchive.SelectedItem = null;
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
                int componentId = Convert.ToInt32(comboBoxBlank.SelectedValue);
                int stockId = Convert.ToInt32(comboBoxArchive.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIKlient.PostRequestData("api/Main/PutBlankOnArchive", new ArchiveBlankBindingModel
                {
                    BlankId = componentId,
                    ArchiveId = stockId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Архив пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

                Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
