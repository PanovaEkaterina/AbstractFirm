using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsBD
{
    public class BlankServiceBD : IBlankService
    {
        private AbstractDbContext context;

        public BlankServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<BlankViewModel> GetList()
        {
            List<BlankViewModel> result = context.Blanks
                .Select(rec => new BlankViewModel
                {
                    Id = rec.Id,
                    BlankName = rec.BlankName
                })
                .ToList();
            return result;
        }

        public BlankViewModel GetElement(int id)
        {
            Blank element = context.Blanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new BlankViewModel
                {
                    Id = element.Id,
                    BlankName = element.BlankName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BlankBindingModel model)
        {
            Blank element = context.Blanks.FirstOrDefault(rec => rec.BlankName == model.BlankName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Blanks.Add(new Blank
            {
                BlankName = model.BlankName
            });
            context.SaveChanges();
        }

        public void UpdElement(BlankBindingModel model)
        {
            Blank element = context.Blanks.FirstOrDefault(rec =>
                                        rec.BlankName == model.BlankName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.Blanks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.BlankName = model.BlankName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Blank element = context.Blanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Blanks.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
