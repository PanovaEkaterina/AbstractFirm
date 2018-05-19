using AbstractFirmService.Attributies;
using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportService
    {
        [CustomMethod("Метод сохранения списка пакетов документов в doc-файл")]
        void SavePackagePrice(ReportBindingModel model);

        [CustomMethod("Метод получения списка архивов с количеством бланков на них")]
        List<ArchivesLoadViewModel> GetArchivesLoad();

        [CustomMethod("Метод сохранения списка архивов с количеством бланков на них в xls-файл")]
        void SaveArchivesLoad(ReportBindingModel model);

        [CustomMethod("Метод получения списка заказов клиентов")]
        List<KlientRequestsModel> GetKlientRequests(ReportBindingModel model);

        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveKlientRequests(ReportBindingModel model);
    }
}
