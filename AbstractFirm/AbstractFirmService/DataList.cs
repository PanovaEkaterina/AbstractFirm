using AbstractFirmModel;
using System.Collections.Generic;

namespace AbstractFirmService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Klient> Klients { get; set; }

        public List<Blank> Blanks { get; set; }

        public List<Lawyer> Lawyers { get; set; }

        public List<Request> Requests { get; set; }

        public List<Package> Packages { get; set; }

        public List<PackageBlank> PackageBlanks { get; set; }

        public List<Archive> Archives { get; set; }

        public List<ArchiveBlank> ArchiveBlanks { get; set; }

        private DataListSingleton()
        {
            Klients = new List<Klient>();
            Blanks = new List<Blank>();
            Lawyers = new List<Lawyer>();
            Requests = new List<Request>();
            Packages = new List<Package>();
            PackageBlanks = new List<PackageBlank>();
            Archives = new List<Archive>();
            ArchiveBlanks = new List<ArchiveBlank>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
