using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IMainService
    {
        List<RequestViewModel> GetList();

        void CreateRequest(RequestBindingModel model);

        void TakeRequestInWork(RequestBindingModel model);

        void FinishRequest(int id);

        void PayRequest(int id);

        void PutBlankOnArchive(ArchiveBlankBindingModel model);
    }
}
