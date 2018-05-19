using AbstractFirmModel;
using System;
using System.Data.Entity;

namespace AbstractFirmService
{
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext() : base("DBFirm")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Klient> Klients { get; set; }

        public virtual DbSet<Blank> Blanks { get; set; }

        public virtual DbSet<Lawyer> Lawyers { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Package> Packages{ get; set; }

        public virtual DbSet<PackageBlank> PackageBlanks { get; set; }

        public virtual DbSet<Archive> Archives { get; set; }

        public virtual DbSet<ArchiveBlank> ArchiveBlanks { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }
    }
}
