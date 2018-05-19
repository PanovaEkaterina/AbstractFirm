using AbstractFirmRestApi.Services;
using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractFirmRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateRequest(RequestBindingModel model)
        {
            _service.CreateRequest(model);
        }

        [HttpPost]
        public void TakeRequestInWork(RequestBindingModel model)
        {
            _service.TakeRequestInWork(model);
        }

        [HttpPost]
        public void FinishRequest(RequestBindingModel model)
        {
            _service.FinishRequest(model.Id);
        }

        [HttpPost]
        public void PayRequest(RequestBindingModel model)
        {
            _service.PayRequest(model.Id);
        }

        [HttpPost]
        public void PutBlankOnArchive(ArchiveBlankBindingModel model)
        {
            _service.PutBlankOnArchive(model);
        }

        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}
