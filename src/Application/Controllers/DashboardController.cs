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

//namespace Application.Controllers
//{
//    public class DashboardController : BaseController
//    {
//        private readonly ILogger<DashboardController> _logger;
//        readonly ICandidateService _candidatureService;
//        public DashboardController(ICandidateService candidatureService)
//        {

//            //_logger = logger;
//            _candidatureService = candidatureService;
//        }

//        public IActionResult Index()
//        {
//            var result = _candidatureService.GetDashboardData();

//            if (result.Success)
//            {
//                var dashboardDTO = (DashboardDTO)result.Object;
//                return View(dashboardDTO);
//            }
//            else
//            {
//                return StatusCode(500, "Erro ao consultar a pagina");
//            }
//            return View(new DashboardDTO());
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
