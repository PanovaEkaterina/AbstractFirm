using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractFirmRestApi.Controllers
{
    public class PackageController : ApiController
    {
        private readonly IPackageService _service;

        public PackageController(IPackageService service)
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
        public void AddElement(PackageBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(PackageBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(PackageBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
