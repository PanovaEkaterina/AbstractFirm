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
    /// Логика взаимодействия для FormLawyer.xaml
    /// </summary>
    public partial class FormLawyer : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ILawyerService service;

        private int? id;

        public FormLawyer(ILawyerService service)
        {
            InitializeComponent();
            Loaded += FormLawyer_Load;
            this.service = service;
        }

        private void FormLawyer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    LawyerViewModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.LawyerFIO;
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
                    service.UpdElement(new LawyerBindingModel
                    {
                        Id = id.Value,
                        LawyerFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new LawyerBindingModel
                    {
                        LawyerFIO = textBoxFullName.Text
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
