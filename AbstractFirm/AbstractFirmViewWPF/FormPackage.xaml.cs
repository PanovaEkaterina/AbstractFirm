using AbstractFirmService.BindingModel;
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
    /// Логика взаимодействия для FormPackage.xaml
    /// </summary>
    public partial class FormPackage : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<PackageBlankViewModel> packageBlanks;

        public FormPackage()
        {
            InitializeComponent();
            Loaded += FormPackage_Load;
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var product = Task.Run(() => APIKlient.GetRequestData<PackageViewModel>("api/PackageProduct/Get/" + id.Value)).Result;
                    textBoxName.Text = product.PackageName;
                    textBoxPrice.Text = product.Price.ToString();
                    packageBlanks = product.PackageBlanks;
                    LoadData();
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
            else
            {
                packageBlanks = new List<PackageBlankViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (packageBlanks != null)
                {
                    dataGridViewPackage.ItemsSource = null;
                    dataGridViewPackage.ItemsSource = packageBlanks;
                    dataGridViewPackage.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewPackage.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormPackageBlank();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.PackageId = id.Value;
                    }           
                    packageBlanks.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackage.SelectedItem != null)
            {
                var form = new FormPackageBlank();
                form.Model = packageBlanks[dataGridViewPackage.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    packageBlanks[dataGridViewPackage.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPackage.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        packageBlanks.RemoveAt(dataGridViewPackage.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (packageBlanks == null || packageBlanks.Count == 0)
            {
                MessageBox.Show("Заполните бланки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                List<PackageBlankBindingModel> productComponentBM = new List<PackageBlankBindingModel>();
                for (int i = 0; i < packageBlanks.Count; ++i)
                {
                    productComponentBM.Add(new PackageBlankBindingModel
                    {
                        Id = packageBlanks[i].Id,
                        PackageId = packageBlanks[i].PackageId,
                        BlankId = packageBlanks[i].BlankId,
                        Count = packageBlanks[i].Count
                    });
                }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Package/UpdElement", new PackageBindingModel
                {
                    Id = id.Value,
                    PackageName = name,
                    Price = price,
                    PackageBlanks = productComponentBM
                }));
            }
            else
            {
                task = Task.Run(() => APIKlient.PostRequestData("api/Package/AddElement", new PackageBindingModel
                {
                    PackageName = name,
                    Price = price,
                    PackageBlanks = productComponentBM
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
