using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IBlankService
    {
        List<BlankViewModel> GetList();

        BlankViewModel GetElement(int id);

        void AddElement(BlankBindingModel model);

        void UpdElement(BlankBindingModel model);

        void DelElement(int id);
    }
}
