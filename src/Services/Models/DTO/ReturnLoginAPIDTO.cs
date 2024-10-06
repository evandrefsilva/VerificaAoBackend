using System;

namespace Services.Models.DTO
{
    public class ReturnLoginAPIDTO
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } // Para JWT
    }
}
