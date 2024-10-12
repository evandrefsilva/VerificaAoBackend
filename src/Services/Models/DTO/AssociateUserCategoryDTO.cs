using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class AssociateUserCategoryDTO
    {
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
    }

}
