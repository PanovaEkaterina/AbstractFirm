using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<RequestViewModel> GetList();

        [CustomMethod("Метод создания заказа")]
        void CreateRequest(RequestBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void TakeRequestInWork(RequestBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishRequest(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayRequest(int id);

        [CustomMethod("Метод пополнения бланка в архиве")]
        void PutBlankOnArchive(ArchiveBlankBindingModel model);
    }
}
