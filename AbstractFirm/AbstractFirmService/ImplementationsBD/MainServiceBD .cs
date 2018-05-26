using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;

namespace AbstractFirmService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = context.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    KlientId = rec.KlientId,
                    PackageId = rec.PackageId,
                    LawyerId = rec.LawyerId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateLawyer = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    KlientFIO = rec.Klient.KlientFIO,
                    PackageName = rec.Package.PackageName,
                    LawyerName = rec.Lawyer.LawyerFIO
                })
                .ToList();
            return result;
        }

        public void CreateRequest(RequestBindingModel model)
        {
            context.Requests.Add(new Request
            {
                KlientId = model.KlientId,
                PackageId = model.PackageId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeRequestInWork(RequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Request element = context.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var packageBlanks = context.PackageBlanks
                                                .Include(rec => rec.Blank)
                                                .Where(rec => rec.PackageId == element.PackageId);
                    // списываем
                    foreach (var productBlank in packageBlanks)
                    {
                        int countOnArchives = productBlank.Count * element.Count;
                        var ArchiveBlanks = context.ArchiveBlanks
                                                    .Where(rec => rec.BlankId == productBlank.BlankId);
                        foreach (var ArchiveBlank in ArchiveBlanks)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (ArchiveBlank.Count >= countOnArchives)
                            {
                                ArchiveBlank.Count -= countOnArchives;
                                countOnArchives = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnArchives -= ArchiveBlank.Count;
                                ArchiveBlank.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnArchives > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                productBlank.Blank.BlankName + " требуется " +
                                productBlank.Count + ", не хватает " + countOnArchives);
                        }
                    }
                    element.LawyerId = model.LawyerId;
                    element.DateImplement = DateTime.Now;
                    element.Status = RequestStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishRequest(int id)
        {
            Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
            context.SaveChanges();
        }

        public void PayRequest(int id)
        {
            Request element = context.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutBlankOnArchive(ArchiveBlankBindingModel model)
        {
            ArchiveBlank element = context.ArchiveBlanks
                                                .FirstOrDefault(rec => rec.ArchiveId == model.ArchiveId &&
                                                                    rec.BlankId == model.BlankId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.ArchiveBlanks.Add(new ArchiveBlank
                {
                    ArchiveId = model.ArchiveId,
                    BlankId = model.BlankId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }
    }
}
