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
        public CategoryDTO(Category tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            IsActive = tag.IsActive;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }



    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
