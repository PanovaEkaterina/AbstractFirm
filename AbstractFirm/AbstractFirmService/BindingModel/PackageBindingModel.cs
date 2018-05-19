using System.Collections.Generic;

namespace AbstractFirmService.BindingModel
{
    public class PackageBindingModel
    {
        public int Id { get; set; }

        public string PackageName { get; set; }

        public decimal Price { get; set; }

        public List<PackageBlankBindingModel> PackageBlanks { get; set; }
    }
}
