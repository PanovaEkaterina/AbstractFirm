using System.Collections.Generic;
namespace AbstractFirmService.ViewModel
{
    public class ArchiveViewModel
    {
        public int Id { get; set; }

        public string ArchiveName { get; set; }

        public List<ArchiveBlankViewModel> ArchiveBlanks { get; set; }
    }
}
