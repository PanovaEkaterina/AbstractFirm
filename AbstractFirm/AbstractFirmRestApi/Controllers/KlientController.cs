using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractFirmRestApi.Controllers
{
    public class KlientController : ApiController
    {
        private readonly IKlientService _service;

        public KlientController(IKlientService service)
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
        public void AddElement(KlientBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(KlientBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(KlientBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
