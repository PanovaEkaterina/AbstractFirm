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
    /// Логика взаимодействия для FormBlank.xaml
    /// </summary>
    public partial class FormBlank : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IBlankService service;

        private int? id;

        public FormBlank(IBlankService service)
        {
            InitializeComponent();
            Loaded += FormBlank_Load;
            this.service = service;
        }

        private void FormBlank_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    BlankViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.BlankName;
                    }
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
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new BlankBindingModel
                    {
                        Id = id.Value,
                        BlankName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new BlankBindingModel
                    {
                        BlankName = textBoxName.Text
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
