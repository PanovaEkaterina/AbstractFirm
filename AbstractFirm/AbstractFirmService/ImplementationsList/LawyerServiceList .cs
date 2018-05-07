using AbstractFirmModel;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using AbstractFirmService.ViewModel;
using System;
using System.Collections.Generic;

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
            List<LawyerViewModel> result = new List<LawyerViewModel>();
            for (int i = 0; i < source.Lawyers.Count; ++i)
            {
                result.Add(new LawyerViewModel
                {
                    Id = source.Lawyers[i].Id,
                    LawyerFIO = source.Lawyers[i].LawyerFIO
                });
            }
            return result;
        }

        public LawyerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Lawyers.Count; ++i)
            {
                if (source.Lawyers[i].Id == id)
                {
                    return new LawyerViewModel
                    {
                        Id = source.Lawyers[i].Id,
                        LawyerFIO = source.Lawyers[i].LawyerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(LawyerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Lawyers.Count; ++i)
            {
                if (source.Lawyers[i].Id > maxId)
                {
                    maxId = source.Lawyers[i].Id;
                }
                if (source.Lawyers[i].LawyerFIO == model.LawyerFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Lawyers.Add(new Lawyer
            {
                Id = maxId + 1,
                LawyerFIO = model.LawyerFIO
            });
        }

        public void UpdElement(LawyerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Lawyers.Count; ++i)
            {
                if (source.Lawyers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Lawyers[i].LawyerFIO == model.LawyerFIO &&
                    source.Lawyers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Lawyers[index].LawyerFIO = model.LawyerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Lawyers.Count; ++i)
            {
                if (source.Lawyers[i].Id == id)
                {
                    source.Lawyers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
