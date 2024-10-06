//using Services.Enums;
//using Services.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Services.Helpers
//{

//    public static class CandidatureStatusExtensions
//    {
//        public static string GetCssClass(this CandidatureStatus status)
//        {
//            switch (status)
//            {
//                case CandidatureStatus.Stage1:
//                    return "stage1"; // Classe CSS para Inscrição
//                case CandidatureStatus.Stage2:
//                    return "stage2"; // Classe CSS para Casting
//                case CandidatureStatus.Stage3:
//                    return "stage3"; // Classe CSS para Gala
//                case CandidatureStatus.Canceled:
//                    return "canceled"; // Classe CSS para Cancelado
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//        public static CandidatureStatusInfo[] ToCandidatureStatusArray()
//        {
//            return Enum.GetValues(typeof(CandidatureStatus))
//                       .Cast<CandidatureStatus>()
//                       .Select(status => new CandidatureStatusInfo
//                       {
//                           Number = (int)status,
//                           Name = status.ToString(),
//                           Description = status.GetDescription(),
//                           CssClass = status.GetCssClass()
//                       })
//                       .ToArray();
//        }
//    }
//}
