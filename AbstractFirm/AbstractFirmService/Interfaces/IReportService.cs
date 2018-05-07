using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IReportService
    {
        void SavePackagePrice(ReportBindingModel model);

        List<ArchivesLoadViewModel> GetArchivesLoad();

        void SaveArchivesLoad(ReportBindingModel model);

        List<KlientRequestsModel> GetKlientRequests(ReportBindingModel model);

        void SaveKlientRequests(ReportBindingModel model);
    }
}
