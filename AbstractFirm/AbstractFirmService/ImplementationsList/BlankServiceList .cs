using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFirmService.ImplementationsList
{
    public class BlankServiceList : IBlankService
    {
        private DataListSingleton source;

        public BlankServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BlankViewModel> GetList()
        {
            List<BlankViewModel> result = source.Blanks
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
            Blank element = source.Blanks.FirstOrDefault(rec => rec.Id == id);
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
            Blank element = source.Blanks.FirstOrDefault(rec => rec.BlankName == model.BlankName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Blanks.Count > 0 ? source.Blanks.Max(rec => rec.Id) : 0;
            source.Blanks.Add(new Blank
            {
                Id = maxId + 1,
                BlankName = model.BlankName
            });
        }

        public void UpdElement(BlankBindingModel model)
        {
            Blank element = source.Blanks.FirstOrDefault(rec =>
                                        rec.BlankName == model.BlankName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Blanks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.BlankName = model.BlankName;
        }

        public void DelElement(int id)
        {
            Blank element = source.Blanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Blanks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
