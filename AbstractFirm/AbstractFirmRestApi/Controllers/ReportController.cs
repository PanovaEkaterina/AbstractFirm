using AbstractFirmService.BindingModel;
using AbstractFirmService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractFirmRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetArchivesLoad()
        {
            var list = _service.GetArchivesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetKlientRequests(ReportBindingModel model)
        {
            var list = _service.GetKlientRequests(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SavePackagePrice(ReportBindingModel model)
        {
            _service.SavePackagePrice(model);
        }

        [HttpPost]
        public void SaveArchivesLoad(ReportBindingModel model)
        {
            _service.SaveArchivesLoad(model);
        }

        [HttpPost]
        public void SaveKlientRequests(ReportBindingModel model)
        {
            _service.SaveKlientRequests(model);
        }
    }
}
