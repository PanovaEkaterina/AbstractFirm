using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IKlientService
    {
        List<KlientViewModel> GetList();

        KlientViewModel GetElement(int id);

        void AddElement(KlientBindingModel model);

        void UpdElement(KlientBindingModel model);

        void DelElement(int id);
    }
}
