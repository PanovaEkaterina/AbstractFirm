using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с клиентом")]
    public interface IKlientService
    {
        [CustomMethod("Метод получения списка клиентов")]
        List<KlientViewModel> GetList();

        [CustomMethod("Метод получения клиента по id")]
        KlientViewModel GetElement(int id);

        [CustomMethod("Метод добавления клиента")]
        void AddElement(KlientBindingModel model);

        [CustomMethod("Метод изменения данных по клиенту")]
        void UpdElement(KlientBindingModel model);

        [CustomMethod("Метод удаления клиента")]
        void DelElement(int id);
    }
}
