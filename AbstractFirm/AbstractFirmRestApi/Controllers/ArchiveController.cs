using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractFirmRestApi.Controllers
{
    public class ArchiveController : ApiController
    {
        private readonly IArchiveService _service;

        public ArchiveController(IArchiveService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(ArchiveBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(ArchiveBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(ArchiveBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
