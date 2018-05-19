using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с пакетами документов")]
    public interface IPackageService
    {
        [CustomMethod("Метод получения списка пакетов документов")]
        List<PackageViewModel> GetList();

        [CustomMethod("Метод получения пакета документов по id")]
        PackageViewModel GetElement(int id);

        [CustomMethod("Метод добавления пакета документов")]
        void AddElement(PackageBindingModel model);

        [CustomMethod("Метод изменения данных по пакету документов")]
        void UpdElement(PackageBindingModel model);

        [CustomMethod("Метод удаления пакета документов")]
        void DelElement(int id);
    }
}
