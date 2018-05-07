using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsBD
{
    public class KlientServiceBD : IKlientService
    {
        private AbstractDbContext context;

        public KlientServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<KlientViewModel> GetList()
        {
            List<KlientViewModel> result = context.Klients
                .Select(rec => new KlientViewModel
                {
                    Id = rec.Id,
                    KlientFIO = rec.KlientFIO
                })
                .ToList();
            return result;
        }

        public KlientViewModel GetElement(int id)
        {
            Klient element = context.Klients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new KlientViewModel
                {
                    Id = element.Id,
                    KlientFIO = element.KlientFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(KlientBindingModel model)
        {
            Klient element = context.Klients.FirstOrDefault(rec => rec.KlientFIO == model.KlientFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Klients.Add(new Klient
            {
                KlientFIO = model.KlientFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(KlientBindingModel model)
        {
            Klient element = context.Klients.FirstOrDefault(rec =>
                                    rec.KlientFIO == model.KlientFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Klients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.KlientFIO = model.KlientFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Klient element = context.Klients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Klients.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
