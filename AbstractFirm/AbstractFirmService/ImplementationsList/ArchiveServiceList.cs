using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;

namespace AbstractFirmService.ImplementationsList
{
    public class ArchiveServiceList : IArchiveService
    {
        private DataListSingleton source;

        public ArchiveServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ArchiveViewModel> GetList()
        {
            List<ArchiveViewModel> result = new List<ArchiveViewModel>();
            for (int i = 0; i < source.Archives.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<ArchiveBlankViewModel> ArchiveBlanks = new List<ArchiveBlankViewModel>();
                for (int j = 0; j < source.ArchiveBlanks.Count; ++j)
                {
                    if (source.ArchiveBlanks[j].ArchiveId == source.Archives[i].Id)
                    {
                        string BlankName = string.Empty;
                        for (int k = 0; k < source.Blanks.Count; ++k)
                        {
                            if (source.PackageBlanks[j].BlankId == source.Blanks[k].Id)
                            {
                                BlankName = source.Blanks[k].BlankName;
                                break;
                            }
                        }
                        ArchiveBlanks.Add(new ArchiveBlankViewModel
                        {
                            Id = source.ArchiveBlanks[j].Id,
                            ArchiveId = source.ArchiveBlanks[j].ArchiveId,
                            BlankId = source.ArchiveBlanks[j].BlankId,
                            BlankName = BlankName,
                            Count = source.ArchiveBlanks[j].Count
                        });
                    }
                }
                result.Add(new ArchiveViewModel
                {
                    Id = source.Archives[i].Id,
                    ArchiveName = source.Archives[i].ArchiveName,
                    ArchiveBlanks = ArchiveBlanks
                });
            }
            return result;
        }

        public ArchiveViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Archives.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<ArchiveBlankViewModel> ArchiveBlanks = new List<ArchiveBlankViewModel>();
                for (int j = 0; j < source.ArchiveBlanks.Count; ++j)
                {
                    if (source.ArchiveBlanks[j].ArchiveId == source.Archives[i].Id)
                    {
                        string BlankName = string.Empty;
                        for (int k = 0; k < source.Blanks.Count; ++k)
                        {
                            if (source.PackageBlanks[j].BlankId == source.Blanks[k].Id)
                            {
                                BlankName = source.Blanks[k].BlankName;
                                break;
                            }
                        }
                        ArchiveBlanks.Add(new ArchiveBlankViewModel
                        {
                            Id = source.ArchiveBlanks[j].Id,
                            ArchiveId = source.ArchiveBlanks[j].ArchiveId,
                            BlankId = source.ArchiveBlanks[j].BlankId,
                            BlankName = BlankName,
                            Count = source.ArchiveBlanks[j].Count
                        });
                    }
                }
                if (source.Archives[i].Id == id)
                {
                    return new ArchiveViewModel
                    {
                        Id = source.Archives[i].Id,
                        ArchiveName = source.Archives[i].ArchiveName,
                        ArchiveBlanks = ArchiveBlanks
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ArchiveBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Archives.Count; ++i)
            {
                if (source.Archives[i].Id > maxId)
                {
                    maxId = source.Archives[i].Id;
                }
                if (source.Archives[i].ArchiveName == model.ArchiveName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Archives.Add(new Archive
            {
                Id = maxId + 1,
                ArchiveName = model.ArchiveName
            });
        }

        public void UpdElement(ArchiveBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Archives.Count; ++i)
            {
                if (source.Archives[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Archives[i].ArchiveName == model.ArchiveName &&
                    source.Archives[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Archives[index].ArchiveName = model.ArchiveName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.ArchiveBlanks.Count; ++i)
            {
                if (source.ArchiveBlanks[i].ArchiveId == id)
                {
                    source.ArchiveBlanks.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Archives.Count; ++i)
            {
                if (source.Archives[i].Id == id)
                {
                    source.Archives.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
