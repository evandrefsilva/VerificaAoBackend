using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class UpdatePasswordDTO
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerifyCode { get; set; }
    }

}
