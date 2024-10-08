using Data.AuthEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.NewsVerfication
{
    public class UserCategory
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
