using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsList
{
    public class KlientServiceList : IKlientService
    {
        private DataListSingleton source;

        public KlientServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<KlientViewModel> GetList()
        {
            List<KlientViewModel> result = source.Klients
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
            Klient element = source.Klients.FirstOrDefault(rec => rec.Id == id);
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
            Klient element = source.Klients.FirstOrDefault(rec => rec.KlientFIO == model.KlientFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            int maxId = source.Klients.Count > 0 ? source.Klients.Max(rec => rec.Id) : 0;
            source.Klients.Add(new Klient
            {
                Id = maxId + 1,
                KlientFIO = model.KlientFIO
            });
        }

        public void UpdElement(KlientBindingModel model)
        {
            Klient element = source.Klients.FirstOrDefault(rec =>
                                    rec.KlientFIO == model.KlientFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = source.Klients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.KlientFIO = model.KlientFIO;
        }

        public void DelElement(int id)
        {
            Klient element = source.Klients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Klients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
