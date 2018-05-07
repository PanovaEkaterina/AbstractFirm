using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface ILawyerService
    {
        List<LawyerViewModel> GetList();

        LawyerViewModel GetElement(int id);

        void AddElement(LawyerBindingModel model);

        void UpdElement(LawyerBindingModel model);

        void DelElement(int id);
    }
}
