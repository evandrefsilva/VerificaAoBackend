using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.NewsVerfication
{
    public class Tag : BaseEntity
    {
        [Required(ErrorMessage = "Tag name is required.")]
        public string Name { get; set; }
        public virtual ICollection<News> NewsArticles { get; set; }
    }
}
