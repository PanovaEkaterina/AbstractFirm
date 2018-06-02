using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormLawyer.xaml
    /// </summary>
    public partial class FormLawyer : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormLawyer()
        {
            InitializeComponent();
            Loaded += FormLawyer_Load;
        }

        private void FormLawyer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var Lawyer = Task.Run(() => APIKlient.GetRequestData<LawyerViewModel>("api/Lawyer/Get/" + id.Value)).Result;
                    textBoxFullName.Text = Lawyer.LawyerFIO;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string fio = textBoxFullName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Lawyer/UpdElement", new LawyerBindingModel
                {
                    Id = id.Value,
                    LawyerFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Lawyer/AddElement", new LawyerBindingModel
                {
                    LawyerFIO = fio
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
