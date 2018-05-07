using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;

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
            List<KlientViewModel> result = new List<KlientViewModel>();
            for (int i = 0; i < source.Klients.Count; ++i)
            {
                result.Add(new KlientViewModel
                {
                    Id = source.Klients[i].Id,
                    KlientFIO = source.Klients[i].KlientFIO
                });
            }
            return result;
        }

        public KlientViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Klients.Count; ++i)
            {
                if (source.Klients[i].Id == id)
                {
                    return new KlientViewModel
                    {
                        Id = source.Klients[i].Id,
                        KlientFIO = source.Klients[i].KlientFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(KlientBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Klients.Count; ++i)
            {
                if (source.Klients[i].Id > maxId)
                {
                    maxId = source.Klients[i].Id;
                }
                if (source.Klients[i].KlientFIO == model.KlientFIO)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Klients.Add(new Klient
            {
                Id = maxId + 1,
                KlientFIO = model.KlientFIO
            });
        }

        public void UpdElement(KlientBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Klients.Count; ++i)
            {
                if (source.Klients[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Klients[i].KlientFIO == model.KlientFIO &&
                    source.Klients[i].Id != model.Id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Klients[index].KlientFIO = model.KlientFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Klients.Count; ++i)
            {
                if (source.Klients[i].Id == id)
                {
                    source.Klients.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
