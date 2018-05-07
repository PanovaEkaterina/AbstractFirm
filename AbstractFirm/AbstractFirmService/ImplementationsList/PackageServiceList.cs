using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsList
{
    public class PackageServiceList : IPackageService
    {
        private DataListSingleton source;

        public PackageServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<PackageViewModel> GetList()
        {
            List<PackageViewModel> result = source.Packages
                .Select(rec => new PackageViewModel
                {
                    Id = rec.Id,
                    PackageName = rec.PackageName,
                    Price = rec.Price,
                    PackageBlanks = source.PackageBlanks
                            .Where(recPC => recPC.PackageId == rec.Id)
                            .Select(recPC => new PackageBlankViewModel
                            {
                                Id = recPC.Id,
                                PackageId = recPC.PackageId,
                                BlankId = recPC.BlankId,
                                BlankName = source.Blanks
                                    .FirstOrDefault(recC => recC.Id == recPC.BlankId)?.BlankName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public PackageViewModel GetElement(int id)
        {
            Package element = source.Packages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PackageViewModel
                {
                    Id = element.Id,
                    PackageName = element.PackageName,
                    Price = element.Price,
                    PackageBlanks = source.PackageBlanks
                            .Where(recPC => recPC.PackageId == element.Id)
                            .Select(recPC => new PackageBlankViewModel
                            {
                                Id = recPC.Id,
                                PackageId = recPC.PackageId,
                                BlankId = recPC.BlankId,
                                BlankName = source.Blanks
                                        .FirstOrDefault(recC => recC.Id == recPC.BlankId)?.BlankName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PackageBindingModel model)
        {
            Package element = source.Packages.FirstOrDefault(rec => rec.PackageName == model.PackageName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.Packages.Count > 0 ? source.Packages.Max(rec => rec.Id) : 0;
            source.Packages.Add(new Package
            {
                Id = maxId + 1,
                PackageName = model.PackageName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.PackageBlanks.Count > 0 ?
                                    source.PackageBlanks.Max(rec => rec.Id) : 0;
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
                source.PackageBlanks.Add(new PackageBlank
                {
                    Id = ++maxPCId,
                    PackageId = maxId + 1,
                    BlankId = groupBlank.BlankId,
                    Count = groupBlank.Count
                });
            }
        }

        public void UpdElement(PackageBindingModel model)
        {
            Package element = source.Packages.FirstOrDefault(rec =>
                                        rec.PackageName == model.PackageName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.Packages.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PackageName = model.PackageName;
            element.Price = model.Price;

            int maxPCId = source.PackageBlanks.Count > 0 ? source.PackageBlanks.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.PackageBlanks.Select(rec => rec.BlankId).Distinct();
            var updateBlanks = source.PackageBlanks
                                            .Where(rec => rec.PackageId == model.Id &&
                                           compIds.Contains(rec.BlankId));
            foreach (var updateBlank in updateBlanks)
            {
                updateBlank.Count = model.PackageBlanks
                                                .FirstOrDefault(rec => rec.Id == updateBlank.Id).Count;
            }
            source.PackageBlanks.RemoveAll(rec => rec.PackageId == model.Id &&
                                       !compIds.Contains(rec.BlankId));
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
                PackageBlank elementPC = source.PackageBlanks
                                        .FirstOrDefault(rec => rec.PackageId == model.Id &&
                                                        rec.BlankId == groupBlank.BlankId);
                if (elementPC != null)
                {
                    elementPC.Count += groupBlank.Count;
                }
                else
                {
                    source.PackageBlanks.Add(new PackageBlank
                    {
                        Id = ++maxPCId,
                        PackageId = model.Id,
                        BlankId = groupBlank.BlankId,
                        Count = groupBlank.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Package element = source.Packages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.PackageBlanks.RemoveAll(rec => rec.PackageId == id);
                source.Packages.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
