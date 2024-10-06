using Data.NewsVerfication;
using System;
using System.Collections.Generic;
using System.Text;
namespace Services.Models.DTO
{
    public class CreateTagDTO
    {
        public string Name { get; set; }
    }

    public class TagDTO
    {
        public TagDTO(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            IsActive = tag.IsActive;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }



    public class UpdateTagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
