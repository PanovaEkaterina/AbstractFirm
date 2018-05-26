using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFirmService.ImplementationsBD
{
    public class PackageServiceBD : IPackageService
    {
        private AbstractDbContext context;

        public PackageServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<PackageViewModel> GetList()
        {
            List<PackageViewModel> result = context.Packages
                .Select(rec => new PackageViewModel
                {
                    Id = rec.Id,
                    PackageName = rec.PackageName,
                    Price = rec.Price,
                    PackageBlanks = context.PackageBlanks
                            .Where(recPC => recPC.PackageId == rec.Id)
                            .Select(recPC => new PackageBlankViewModel
                            {
                                Id = recPC.Id,
                                PackageId = recPC.PackageId,
                                BlankId = recPC.BlankId,
                                BlankName = recPC.Blank.BlankName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public PackageViewModel GetElement(int id)
        {
            Package element = context.Packages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PackageViewModel
                {
                    Id = element.Id,
                    PackageName = element.PackageName,
                    Price = element.Price,
                    PackageBlanks = context.PackageBlanks
                            .Where(recPC => recPC.PackageId == element.Id)
                            .Select(recPC => new PackageBlankViewModel
                            {
                                Id = recPC.Id,
                                PackageId = recPC.PackageId,
                                BlankId = recPC.BlankId,
                                BlankName = recPC.Blank.BlankName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PackageBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Package element = context.Packages.FirstOrDefault(rec => rec.PackageName == model.PackageName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Package
                    {
                        PackageName = model.PackageName,
                        Price = model.Price
                    };
                    context.Packages.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupBlanks = model.PackageBlanks
                                                .GroupBy(rec => rec.BlankId)
                                                .Select(rec => new
                                                {
                                                    BlankId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    // добавляем компоненты
                    foreach (var groupBlank in groupBlanks)
                    {
                        context.PackageBlanks.Add(new PackageBlank
                        {
                            PackageId = element.Id,
                            BlankId = groupBlank.BlankId,
                            Count = groupBlank.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(PackageBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Package element = context.Packages.FirstOrDefault(rec =>
                                        rec.PackageName == model.PackageName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Packages.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.PackageName = model.PackageName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.PackageBlanks.Select(rec => rec.BlankId).Distinct();
                    var updateBlanks = context.PackageBlanks
                                                    .Where(rec => rec.PackageId == model.Id &&
                                                        compIds.Contains(rec.BlankId));
                    foreach (var updateBlank in updateBlanks)
                    {
                        updateBlank.Count = model.PackageBlanks
                                                        .FirstOrDefault(rec => rec.Id == updateBlank.Id).Count;
                    }
                    context.SaveChanges();
                    context.PackageBlanks.RemoveRange(
                                        context.PackageBlanks.Where(rec => rec.PackageId == model.Id &&
                                                                            !compIds.Contains(rec.BlankId)));
                    context.SaveChanges();
                    // новые записи
                    var groupBlanks = model.PackageBlanks
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.BlankId)
                                                .Select(rec => new
                                                {
                                                    BlankId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupBlank in groupBlanks)
                    {
                        PackageBlank elementPC = context.PackageBlanks
                                                .FirstOrDefault(rec => rec.PackageId == model.Id &&
                                                                rec.BlankId == groupBlank.BlankId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupBlank.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.PackageBlanks.Add(new PackageBlank
                            {
                                PackageId = model.Id,
                                BlankId = groupBlank.BlankId,
                                Count = groupBlank.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Package element = context.Packages.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.PackageBlanks.RemoveRange(
                                            context.PackageBlanks.Where(rec => rec.PackageId == id));
                        context.Packages.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
