using System;

namespace Services.Models.DTO
{
    public class LikeDTO
    {
        public int NewsId { get; set; }
        public Guid UserId { get; private set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}
