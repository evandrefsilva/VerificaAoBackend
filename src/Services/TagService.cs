using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.NewsVerfication;
using Microsoft.EntityFrameworkCore;
using Services.Models;
using Services.Models.DTO;

namespace Services
{
    public interface ITagService
    {
        Task<AppResult> CreateTag(CreateTagDTO dto);
        Task<AppResult> UpdateTag(UpdateTagDTO dto);
        Task<AppResult> DeleteTag(int tagId);
        Task<AppResult> GetAllTags();
        Task<AppResult> GetActiveTags();
        Task<AppResult> GetTagById(int Id);
    }


    public class TagService : ITagService
    {
        private readonly DataContext _db;

        public TagService(DataContext db)
        {
            _db = db;
        }

        public async Task<AppResult> CreateTag(CreateTagDTO dto)
        {
            var res = new AppResult();

            var existingTag = await _db.Tags.FirstOrDefaultAsync(x => x.Name == dto.Name);
            if (existingTag != null)
                return res.Bad("Tag já existe com este nome.");

            var tag = new Tag
            {
                Name = dto.Name,
                IsActive = true // Ativa por padrão
            };

            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();

            return res.Good("Tag criada com sucesso.");
        }

        public async Task<AppResult> UpdateTag(UpdateTagDTO dto)
        {
            var res = new AppResult();

            var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (tag == null)
                return res.Bad("Tag não encontrada.");

            tag.Name = dto.Name;
            tag.IsActive = dto.IsActive;

            _db.Tags.Update(tag);
            await _db.SaveChangesAsync();

            return res.Good("Tag atualizada com sucesso.");
        }

        public async Task<AppResult> DeleteTag(int tagId)
        {
            var res = new AppResult();

            var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Id == tagId);
            if (tag == null)
                return res.Bad("Tag não encontrada.");

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();

            return res.Good("Tag removida com sucesso.");
        }

        //public async Task<AppResult> AddTagToItem(AddTagToItemDTO dto)
        //{
        //    var res = new AppResult();

        //    var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Id == dto.TagId);
        //    if (tag == null)
        //        return res.Bad("Tag não encontrada.");

        //    var item = await _db.Items.FirstOrDefaultAsync(x => x.Id == dto.ItemId);
        //    if (item == null)
        //        return res.Bad("Item não encontrado.");

        //    // Assumindo que existe uma relação entre Tags e Items
        //    item.Tags.Add(tag);

        //    _db.Items.Update(item);
        //    await _db.SaveChangesAsync();

        //    return res.Good("Tag adicionada ao item com sucesso.");
        //}

        public async Task<AppResult> GetAllTags()
        {
            var tags = await _db.Tags
                .Select(x => new TagDTO(x))
                .ToListAsync();
            return new AppResult().Good("Lista de tags", tags);
        }

        public async Task<AppResult> GetTagById(int id)
        {
            var res = new AppResult();

            // Busca a tag no banco de dados usando o ID
            var tag = await _db.Tags
                            .FirstOrDefaultAsync(t => t.Id == id);

            // Verifica se a tag existe
            if (tag == null)
                return res.Bad("Tag não encontrada.");

            // Retorna a tag encontrada
            return res.Good("Tag encontrada.", new TagDTO(tag));
        }

        public async Task<AppResult> GetActiveTags()
        {
            var activeTags = await _db.Tags
                .Where(x => x.IsActive)
                .Select(x => new TagDTO(x))
                .ToListAsync();
            return new AppResult().Good("Lista de tags ativas", activeTags);
        }
    }
}