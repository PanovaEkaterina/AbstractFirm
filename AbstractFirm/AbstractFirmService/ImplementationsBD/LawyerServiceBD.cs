using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsBD
{
    public class LawyerServiceBD : ILawyerService
    {
        private AbstractDbContext context;

        public LawyerServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<LawyerViewModel> GetList()
        {
            List<LawyerViewModel> result = context.Lawyers
                .Select(rec => new LawyerViewModel
                {
                    Id = rec.Id,
                    LawyerFIO = rec.LawyerFIO
                })
                .ToList();
            return result;
        }

        public LawyerViewModel GetElement(int id)
        {
            Lawyer element = context.Lawyers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new LawyerViewModel
                {
                    Id = element.Id,
                    LawyerFIO = element.LawyerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(LawyerBindingModel model)
        {
            Lawyer element = context.Lawyers.FirstOrDefault(rec => rec.LawyerFIO == model.LawyerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Lawyers.Add(new Lawyer
            {
                LawyerFIO = model.LawyerFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(LawyerBindingModel model)
        {
            Lawyer element = context.Lawyers.FirstOrDefault(rec =>
                                        rec.LawyerFIO == model.LawyerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Lawyers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.LawyerFIO = model.LawyerFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Lawyer element = context.Lawyers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Lawyers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
