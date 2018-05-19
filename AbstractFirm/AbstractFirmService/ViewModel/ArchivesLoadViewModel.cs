using System;
using System.Collections.Generic;

namespace AbstractFirmService.ViewModel
{
    public class ArchivesLoadViewModel
    {
        public string ArchiveName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Blanks { get; set; }
    }
}
