using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.NewsVerfication
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public virtual ICollection<News> NewsArticles { get; set; }
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}
