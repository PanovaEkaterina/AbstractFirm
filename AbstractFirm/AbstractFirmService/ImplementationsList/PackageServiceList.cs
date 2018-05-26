using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;

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
            List<PackageViewModel> result = new List<PackageViewModel>();
            for (int i = 0; i < source.Packages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<PackageBlankViewModel> packageBlanks = new List<PackageBlankViewModel>();
                for (int j = 0; j < source.PackageBlanks.Count; ++j)
                {
                    if (source.PackageBlanks[j].PackageId == source.Packages[i].Id)
                    {
                        string blankName = string.Empty;
                        for (int k = 0; k < source.Blanks.Count; ++k)
                        {
                            if (source.PackageBlanks[j].BlankId == source.Blanks[k].Id)
                            {
                                blankName = source.Blanks[k].BlankName;
                                break;
                            }
                        }
                        packageBlanks.Add(new PackageBlankViewModel
                        {
                            Id = source.PackageBlanks[j].Id,
                            PackageId = source.PackageBlanks[j].PackageId,
                            BlankId = source.PackageBlanks[j].BlankId,
                            BlankName = blankName,
                            Count = source.PackageBlanks[j].Count
                        });
                    }
                }
                result.Add(new PackageViewModel
                {
                    Id = source.Packages[i].Id,
                    PackageName = source.Packages[i].PackageName,
                    Price = source.Packages[i].Price,
                    PackageBlanks = packageBlanks
                });
            }
            return result;
        }

        public PackageViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Packages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<PackageBlankViewModel> packageBlanks = new List<PackageBlankViewModel>();
                for (int j = 0; j < source.PackageBlanks.Count; ++j)
                {
                    if (source.PackageBlanks[j].PackageId == source.Packages[i].Id)
                    {
                        string blankName = string.Empty;
                        for (int k = 0; k < source.Blanks.Count; ++k)
                        {
                            if (source.PackageBlanks[j].BlankId == source.Blanks[k].Id)
                            {
                                blankName = source.Blanks[k].BlankName;
                                break;
                            }
                        }
                        packageBlanks.Add(new PackageBlankViewModel
                        {
                            Id = source.PackageBlanks[j].Id,
                            PackageId = source.PackageBlanks[j].PackageId,
                            BlankId = source.PackageBlanks[j].BlankId,
                            BlankName = blankName,
                            Count = source.PackageBlanks[j].Count
                        });
                    }
                }
                if (source.Packages[i].Id == id)
                {
                    return new PackageViewModel
                    {
                        Id = source.Packages[i].Id,
                        PackageName = source.Packages[i].PackageName,
                        Price = source.Packages[i].Price,
                        PackageBlanks = packageBlanks
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(PackageBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Packages.Count; ++i)
            {
                if (source.Packages[i].Id > maxId)
                {
                    maxId = source.Packages[i].Id;
                }
                if (source.Packages[i].PackageName == model.PackageName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Packages.Add(new Package
            {
                Id = maxId + 1,
                PackageName = model.PackageName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].Id > maxPCId)
                {
                    maxPCId = source.PackageBlanks[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.PackageBlanks.Count; ++i)
            {
                for (int j = 1; j < model.PackageBlanks.Count; ++j)
                {
                    if (model.PackageBlanks[i].BlankId ==
                        model.PackageBlanks[j].BlankId)
                    {
                        model.PackageBlanks[i].Count +=
                            model.PackageBlanks[j].Count;
                        model.PackageBlanks.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.PackageBlanks.Count; ++i)
            {
                source.PackageBlanks.Add(new PackageBlank
                {
                    Id = ++maxPCId,
                    PackageId = maxId + 1,
                    BlankId = model.PackageBlanks[i].BlankId,
                    Count = model.PackageBlanks[i].Count
                });
            }
        }

        public void UpdElement(PackageBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Packages.Count; ++i)
            {
                if (source.Packages[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Packages[i].PackageName == model.PackageName &&
                    source.Packages[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Packages[index].PackageName = model.PackageName;
            source.Packages[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].Id > maxPCId)
                {
                    maxPCId = source.PackageBlanks[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].PackageId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.PackageBlanks.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.PackageBlanks[i].Id == model.PackageBlanks[j].Id)
                        {
                            source.PackageBlanks[i].Count = model.PackageBlanks[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.PackageBlanks.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.PackageBlanks.Count; ++i)
            {
                if (model.PackageBlanks[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.PackageBlanks.Count; ++j)
                    {
                        if (source.PackageBlanks[j].PackageId == model.Id &&
                            source.PackageBlanks[j].BlankId == model.PackageBlanks[i].BlankId)
                        {
                            source.PackageBlanks[j].Count += model.PackageBlanks[i].Count;
                            model.PackageBlanks[i].Id = source.PackageBlanks[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.PackageBlanks[i].Id == 0)
                    {
                        source.PackageBlanks.Add(new PackageBlank
                        {
                            Id = ++maxPCId,
                            PackageId = model.Id,
                            BlankId = model.PackageBlanks[i].BlankId,
                            Count = model.PackageBlanks[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].PackageId == id)
                {
                    source.PackageBlanks.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Packages.Count; ++i)
            {
                if (source.Packages[i].Id == id)
                {
                    source.Packages.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
