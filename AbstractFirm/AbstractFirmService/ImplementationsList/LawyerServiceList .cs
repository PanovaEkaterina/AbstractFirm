using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsList
{
    public class LawyerServiceList : ILawyerService
    {
        private DataListSingleton source;

        public LawyerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<LawyerViewModel> GetList()
        {
            List<LawyerViewModel> result = source.Lawyers
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
            Lawyer element = source.Lawyers.FirstOrDefault(rec => rec.Id == id);
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
            Lawyer element = source.Lawyers.FirstOrDefault(rec => rec.LawyerFIO == model.LawyerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Lawyers.Count > 0 ? source.Lawyers.Max(rec => rec.Id) : 0;
            source.Lawyers.Add(new Lawyer
            {
                Id = maxId + 1,
                LawyerFIO = model.LawyerFIO
            });
        }

        public void UpdElement(LawyerBindingModel model)
        {
            Lawyer element = source.Lawyers.FirstOrDefault(rec =>
                                        rec.LawyerFIO == model.LawyerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Lawyers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.LawyerFIO = model.LawyerFIO;
        }

        public void DelElement(int id)
        {
            Lawyer element = source.Lawyers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Lawyers.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
