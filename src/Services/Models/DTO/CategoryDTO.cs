using Data.NewsVerfication;
using System;
using System.Collections.Generic;
using System.Text;
namespace Services.Models.DTO
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
    }

    public class CategoryDTO
    {
        public CategoryDTO(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Slug = category.Slug;
            IsActive = category.IsActive;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
    }



    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
