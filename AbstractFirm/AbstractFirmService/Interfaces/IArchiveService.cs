using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с архивом")]
    public interface IArchiveService
    {
        [CustomMethod("Метод получения списка архивов")]
        List<ArchiveViewModel> GetList();

        [CustomMethod("Метод получения архива по id")]
        ArchiveViewModel GetElement(int id);

        [CustomMethod("Метод добавления архива")]
        void AddElement(ArchiveBindingModel model);

        [CustomMethod("Метод изменения данных по архиву")]
        void UpdElement(ArchiveBindingModel model);

        [CustomMethod("Метод удаления архива")]
        void DelElement(int id);
    }
}
