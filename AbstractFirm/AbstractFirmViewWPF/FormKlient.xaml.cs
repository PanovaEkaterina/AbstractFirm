using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractFirmViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormKlient.xaml
    /// </summary>
    public partial class FormKlient : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IKlientService service;

        private int? id;

        public FormKlient(IKlientService service)
        {
            InitializeComponent();
            Loaded += FormKlient_Load;
            this.service = service;
        }

        private void FormKlient_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    KlientViewModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.KlientFIO;
                }
                catch (Exception ex)
                {
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
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new KlientBindingModel
                    {
                        Id = id.Value,
                        KlientFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new KlientBindingModel
                    {
                        KlientFIO = textBoxFullName.Text
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
