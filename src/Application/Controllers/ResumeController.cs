//using Microsoft.AspNetCore.Mvc;
//using Services;
//using Services.Models.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Application.Controllers
//{
//    public class ResumeController : Controller
//    {
//        private readonly ICandidateService _candidatureService;
//        public ResumeController(ICandidateService candidateService)
//        {
//            _candidatureService = candidateService;
//        }

//        [HttpGet("resume/{Id}")]
//        public IActionResult Index(int Id)
//        {
//            var result = _candidatureService.GetDetails(Id);
//            if (!result.Success)
//                return StatusCode(404);
//            return View((CandidateDTOOutput)result.Object);
//        }
//    }
//}
