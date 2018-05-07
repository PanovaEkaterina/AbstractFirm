using System.Collections.Generic;

namespace AbstractFirmService.ViewModel
{
    public class PackageViewModel
    {
        public int Id { get; set; }

        public string PackageName { get; set; }

        public decimal Price { get; set; }

        public List<PackageBlankViewModel> PackageBlanks { get; set; }
    }
}
