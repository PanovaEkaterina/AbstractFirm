using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
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
                var responseC = APIKlient.GetRequest("api/Blank/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<BlankViewModel> list = APIKlient.GetElement<List<BlankViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxBlank.DisplayMemberPath = "BlankName";
                        comboBoxBlank.SelectedValuePath = "Id";
                        comboBoxBlank.ItemsSource = list;
                        comboBoxBlank.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIKlient.GetError(responseC));
                }
                var responseS = APIKlient.GetRequest("api/Archive/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<ArchiveViewModel> list = APIKlient.GetElement<List<ArchiveViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxArchive.DisplayMemberPath = "ArchiveName";
                        comboBoxArchive.SelectedValuePath = "Id";
                        comboBoxArchive.ItemsSource = list;
                        comboBoxArchive.SelectedItem = null;
                    }
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
                var response = APIKlient.PostRequest("api/Main/PutBlankOnArchive", new ArchiveBlankBindingModel
                {
                    BlankId = Convert.ToInt32(comboBoxBlank.SelectedValue),
                    ArchiveId = Convert.ToInt32(comboBoxArchive.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APIKlient.GetError(response));
                }
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
