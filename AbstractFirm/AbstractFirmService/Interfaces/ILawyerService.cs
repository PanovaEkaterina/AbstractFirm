using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с юристом")]
    public interface ILawyerService
    {
        [CustomMethod("Метод получения списка юристов")]
        List<LawyerViewModel> GetList();

        [CustomMethod("Метод получения юриста по id")]
        LawyerViewModel GetElement(int id);

        [CustomMethod("Метод добавления юриста")]
        void AddElement(LawyerBindingModel model);

        [CustomMethod("Метод изменения данных по юристу")]
        void UpdElement(LawyerBindingModel model);

        [CustomMethod("Метод удаления юриста")]
        void DelElement(int id);
    }
}
