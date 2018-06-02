using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using AbstractFirmView;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormKlient.xaml
    /// </summary>
    public partial class FormKlient : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormKlient()
        {
            InitializeComponent();
            Loaded += FormKlient_Load;
        }

        private void FormKlient_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {  
                    var klient = Task.Run(() => APIKlient.GetRequestData<KlientViewModel>("api/Klient/Get/" + id.Value)).Result;
                    textBoxFullName.Text = klient.KlientFIO;          
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
                task = Task.Run(() => APIKlient.PostRequestData("api/Klient/UpdElement", new KlientBindingModel
                {
                    Id = id.Value,
                    KlientFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Klient/AddElement", new KlientBindingModel
                {
                    KlientFIO = fio
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
