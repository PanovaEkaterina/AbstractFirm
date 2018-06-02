using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormTakeRequestInWork.xaml
    /// </summary>
    public partial class FormTakeRequestInWork : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormTakeRequestInWork()
        {
            InitializeComponent();
            Loaded += FormTakeRequestInWork_Load;
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
                var response = APIKlient.GetRequest("api/Lawyer/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<LawyerViewModel> list = APIKlient.GetElement<List<LawyerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxLawyer.DisplayMemberPath = "LawyerFIO";
                        comboBoxLawyer.SelectedValuePath = "Id";
                        comboBoxLawyer.ItemsSource = list;
                        comboBoxLawyer.SelectedItem = null;

                    }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxLawyer.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIKlient.PostRequest("api/Main/TakeRequestInWork", new RequestBindingModel
                {
                    Id = id.Value,
                    LawyerId = ((LawyerViewModel)comboBoxLawyer.SelectedItem).Id,
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
