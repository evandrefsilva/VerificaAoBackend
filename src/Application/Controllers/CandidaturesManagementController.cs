//using Application.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Services;
//using Services.Models.DTO;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using X.PagedList;

//namespace Application.Controllers
//{
//    public class CandidaturesManagementController : BaseController
//    {
//        readonly ICandidateService _candidatureService;
//        public CandidaturesManagementController(ICandidateService candidatureService)
//        {
//            _candidatureService = candidatureService;
//        }
//        public IActionResult Index(int page = 1, int take = 30
//            , string search = null
//            , DateTime? begin = null
//            , DateTime? end = null
//            , int statusId = 0
//            , int provinceId = 0)
//        {




//            var result = _candidatureService.GetAll(page, take, search, begin, end, statusId, provinceId);

//            if (result.Success)
//            {
//                ViewBag.Search = search;
//                ViewBag.Begin = begin;
//                ViewBag.End = end;
//                ViewBag.StatusId = statusId;
//                ViewBag.ProvinceId = provinceId;
//                var dataColletion = (PagedList<CandidateDTOOutput>)result.Object;
//                return View(dataColletion);
//            }
//            else
//            {
//                return StatusCode(500, "Erro ao consultar a pagina");
//            }
//        }
//        public IActionResult ExportToExcel(string filter = null, DateTime? begin = null, DateTime? end = null, int statusId = 0, int provinceId = 0)
//        {
//            var result = _candidatureService.GetAllAsExcel(filter, begin, end, statusId, provinceId);

//            if (result.Success)
//            {
//                return File((byte[])result.Object, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "candidates.xlsx");
//            }
//            else
//            {
//                // Handle error case
//                return BadRequest(result.Message);
//            }
//        }
//        public IActionResult Details(int Id)
//        {
//            var result = _candidatureService.GetDetails(Id);

//            if (result.Success)
//            {
//                var dataColletion = (CandidateDTOOutput)result.Object;
//                return View(dataColletion);
//            }
//            else
//            {
//                return StatusCode(500, "Erro ao consultar a pagina");
//            }
//        }
//        public IActionResult ChangeState([FromRoute] int Id, [FromQuery] int statusId)
//        {
//            var result = _candidatureService.ChangeState(Id, statusId);

//            if (result.Success)
//            {
//                return RedirectToAction("Details", new { Id = Id });
//            }
//            else
//            {
//                return StatusCode(500, "Erro ao consultar a pagina");
//            }

//        }
//    }
//}
