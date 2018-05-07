using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<RequestViewModel> result = source.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    KlientId = rec.KlientId,
                    PackageId = rec.PackageId,
                    LawyerId = rec.LawyerId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateLawyer = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    KlientFIO = source.Klients
                                    .FirstOrDefault(recC => recC.Id == rec.KlientId)?.KlientFIO,
                    PackageName = source.Packages
                                    .FirstOrDefault(recP => recP.Id == rec.PackageId)?.PackageName,
                    LawyerName = source.Lawyers
                                    .FirstOrDefault(recI => recI.Id == rec.LawyerId)?.LawyerFIO
                })
                .ToList();
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            int maxId = source.Requests.Count > 0 ? source.Requests.Max(rec => rec.Id) : 0;
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
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var productBlanks = source.PackageBlanks.Where(rec => rec.PackageId == element.PackageId);
            foreach (var productBlank in productBlanks)
            {
                int countOnArchives = source.ArchiveBlanks
                                            .Where(rec => rec.BlankId == productBlank.BlankId)
                                            .Sum(rec => rec.Count);
                if (countOnArchives < productBlank.Count * element.Count)
                {
                    var BlankName = source.Blanks
                                    .FirstOrDefault(rec => rec.Id == productBlank.BlankId);
                    throw new Exception("Не достаточно компонента " + BlankName?.BlankName +
                        " требуется " + productBlank.Count + ", в наличии " + countOnArchives);
                }
            }
            // списываем
            foreach (var productBlank in productBlanks)
            {
                int countOnArchives = productBlank.Count * element.Count;
                var ArchiveBlanks = source.ArchiveBlanks
                                            .Where(rec => rec.BlankId == productBlank.BlankId);
                foreach (var ArchiveBlank in ArchiveBlanks)
                {
                    // компонентов на одном слкаде может не хватать
                    if (ArchiveBlank.Count >= countOnArchives)
                    {
                        ArchiveBlank.Count -= countOnArchives;
                        break;
                    }
                    else
                    {
                        countOnArchives -= ArchiveBlank.Count;
                        ArchiveBlank.Count = 0;
                    }
                }
            }
            element.LawyerId = model.LawyerId;
            element.DateImplement = DateTime.Now;
            element.Status = RequestStatus.Выполняется;
        }

        public void FinishRequest(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
        }

        public void PayRequest(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
        }

        public void PutBlankOnArchive(ArchiveBlankBindingModel model)
        {
            ArchiveBlank element = source.ArchiveBlanks
                                                .FirstOrDefault(rec => rec.ArchiveId == model.ArchiveId &&
                                                                    rec.BlankId == model.BlankId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.ArchiveBlanks.Count > 0 ? source.ArchiveBlanks.Max(rec => rec.Id) : 0;
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
}
