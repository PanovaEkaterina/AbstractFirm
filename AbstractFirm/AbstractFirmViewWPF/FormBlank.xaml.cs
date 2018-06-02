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
    /// Логика взаимодействия для FormBlank.xaml
    /// </summary>
    public partial class FormBlank : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormBlank()
        {
            InitializeComponent();
            Loaded += FormBlank_Load;
        }

        private void FormBlank_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                        var blank = Task.Run(() => APIKlient.GetRequestData<BlankViewModel>("api/Blank/Get/" + id.Value)).Result;
                    textBoxName.Text = blank.BlankName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Blank/UpdElement", new BlankBindingModel
                {
                    Id = id.Value,
                    BlankName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Blank/AddElement", new BlankBindingModel
                {
                    BlankName = name
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
