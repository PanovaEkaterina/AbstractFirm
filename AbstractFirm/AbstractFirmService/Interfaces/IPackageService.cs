using AbstractFirmService.BindingModel;
using AbstractFirmService.ViewModel;
using System.Collections.Generic;

namespace AbstractFirmService.Interfaces
{
    public interface IPackageService
    {
        List<PackageViewModel> GetList();

        PackageViewModel GetElement(int id);

        void AddElement(PackageBindingModel model);

        void UpdElement(PackageBindingModel model);

        void DelElement(int id);
    }
}
