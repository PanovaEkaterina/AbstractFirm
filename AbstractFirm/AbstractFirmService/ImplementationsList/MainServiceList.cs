using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;

namespace AbstractFirmService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = new List<RequestViewModel>();
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                string KlientFIO = string.Empty;
                for (int j = 0; j < source.Klients.Count; ++j)
                {
                    if (source.Klients[j].Id == source.Requests[i].KlientId)
                    {
                        KlientFIO = source.Klients[j].KlientFIO;
                        break;
                    }
                }
                string packageName= string.Empty;
                for (int j = 0; j < source.Packages.Count; ++j)
                {
                    if (source.Packages[j].Id == source.Requests[i].PackageId)
                    {
                        packageName= source.Packages[j].PackageName;
                        break;
                    }
                }
                string lawyerFIO = string.Empty;
                if (source.Requests[i].LawyerId.HasValue)
                {
                    for (int j = 0; j < source.Lawyers.Count; ++j)
                    {
                        if (source.Lawyers[j].Id == source.Requests[i].LawyerId.Value)
                        {
                            lawyerFIO = source.Lawyers[j].LawyerFIO;
                            break;
                        }
                    }
                }
                result.Add(new RequestViewModel
                {
                    Id = source.Requests[i].Id,
                    KlientId = source.Requests[i].KlientId,
                    KlientFIO = KlientFIO,
                    PackageId = source.Requests[i].PackageId,
                    PackageName = packageName,
                    LawyerId = source.Requests[i].LawyerId,
                    LawyerName = lawyerFIO,
                    Count = source.Requests[i].Count,
                    Sum = source.Requests[i].Sum,
                    DateCreate = source.Requests[i].DateCreate.ToLongDateString(),
                    DateLawyer = source.Requests[i].DateImplement?.ToLongDateString(),
                    Status = source.Requests[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id > maxId)
                {
                    maxId = source.Klients[i].Id;
                }
            }
            source.Requests.Add(new Request
            {
                Id = maxId + 1,
                KlientId = model.KlientId,
                PackageId = model.PackageId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
        }

        public void TakeRequestInWork(RequestBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].PackageId == source.Requests[index].PackageId)
                {
                    int countOnArchives = 0;
                    for (int j = 0; j < source.ArchiveBlanks.Count; ++j)
                    {
                        if (source.ArchiveBlanks[j].BlankId == source.PackageBlanks[i].BlankId)
                        {
                            countOnArchives += source.ArchiveBlanks[j].Count;
                        }
                    }
                    if (countOnArchives < source.PackageBlanks[i].Count * source.Requests[index].Count)
                    {
                        for (int j = 0; j < source.Blanks.Count; ++j)
                        {
                            if (source.Blanks[j].Id == source.PackageBlanks[i].BlankId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Blanks[j].BlankName +
                                    " требуется " + source.PackageBlanks[i].Count + ", в наличии " + countOnArchives);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.PackageBlanks.Count; ++i)
            {
                if (source.PackageBlanks[i].PackageId == source.Requests[index].PackageId)
                {
                    int countOnArchives = source.PackageBlanks[i].Count * source.Requests[index].Count;
                    for (int j = 0; j < source.ArchiveBlanks.Count; ++j)
                    {
                        if (source.ArchiveBlanks[j].BlankId == source.PackageBlanks[i].BlankId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.ArchiveBlanks[j].Count >= countOnArchives)
                            {
                                source.ArchiveBlanks[j].Count -= countOnArchives;
                                break;
                            }
                            else
                            {
                                countOnArchives -= source.ArchiveBlanks[j].Count;
                                source.ArchiveBlanks[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Requests[index].LawyerId = model.LawyerId;
            source.Requests[index].DateImplement = DateTime.Now;
            source.Requests[index].Status = RequestStatus.Выполняется;
        }

        public void FinishRequest(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Klients[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Готов;
        }

        public void PayRequest(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Klients[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Оплачен;
        }

        public void PutBlankOnArchive(ArchiveBlankBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.ArchiveBlanks.Count; ++i)
            {
                if (source.ArchiveBlanks[i].ArchiveId == model.ArchiveId &&
                    source.ArchiveBlanks[i].BlankId == model.BlankId)
                {
                    source.ArchiveBlanks[i].Count += model.Count;
                    return;
                }
                if (source.ArchiveBlanks[i].Id > maxId)
                {
                    maxId = source.ArchiveBlanks[i].Id;
                }
            }
            source.ArchiveBlanks.Add(new ArchiveBlank
            {
                Id = ++maxId,
                ArchiveId = model.ArchiveId,
                BlankId = model.BlankId,
                Count = model.Count
            });
        }
    }
}
