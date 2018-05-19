using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с бланком")]
    public interface IBlankService
    {
        [CustomMethod("Метод получения списка бланков")]
        List<BlankViewModel> GetList();

        [CustomMethod("Метод получения бланка по id")]
        BlankViewModel GetElement(int id);

        [CustomMethod("Метод добавления бланка")]
        void AddElement(BlankBindingModel model);

        [CustomMethod("Метод изменения данных по бланку")]
        void UpdElement(BlankBindingModel model);

        [CustomMethod("Метод удаления бланка")]
        void DelElement(int id);
    }
}
