using System;

namespace Services.Models.DTO
{
    public class LikeDTO
    {
        public int NewsId { get; set; }
        public Guid UserId { get; set; }
    }
}
