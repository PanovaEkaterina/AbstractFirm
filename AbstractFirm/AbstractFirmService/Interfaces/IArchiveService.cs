using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IArchiveService
    {
        List<ArchiveViewModel> GetList();

        ArchiveViewModel GetElement(int id);

        void AddElement(ArchiveBindingModel model);

        void UpdElement(ArchiveBindingModel model);

        void DelElement(int id);
    }
}
