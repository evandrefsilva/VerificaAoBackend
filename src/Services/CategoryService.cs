using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.NewsVerfication;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Services.Models;
using Services.Models.DTO;

namespace Services
{
    public interface ICategoryService
    {
        Task<AppResult> CreateCategory(CreateCategoryDTO dto);
        Task<AppResult> UpdateCategory(UpdateCategoryDTO dto);
        Task<AppResult> DeleteCategory(int tagId);
        Task<AppResult> GetAllCategorys();
        Task<AppResult> GetActiveCategorys();
        Task<AppResult> GetCategoryById(int Id);
        Task<AppResult> AssociateUserWithCategory(AssociateUserCategoryDTO dto);
    }
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _db;

        public CategoryService(DataContext db)
        {
            _db = db;
        }

        public async Task<AppResult> CreateCategory(CreateCategoryDTO dto)
        {
            var res = new AppResult();

            var existingCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Name == dto.Name);
            if (existingCategory != null)
                return res.Bad("Category já existe com este nome.");

            var tag = new Category
            {
                Name = dto.Name,
                IsActive = true // Ativa por padrão
            };

            await _db.Categories.AddAsync(tag);
            await _db.SaveChangesAsync();

            return res.Good("Category criada com sucesso.");
        }

        public async Task<AppResult> UpdateCategory(UpdateCategoryDTO dto)
        {
            var res = new AppResult();

            var tag = await _db.Categories.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (tag == null)
                return res.Bad("Category não encontrada.");

            tag.Name = dto.Name;
            tag.IsActive = dto.IsActive;

            _db.Categories.Update(tag);
            await _db.SaveChangesAsync();

            return res.Good("Category atualizada com sucesso.");
        }

        public async Task<AppResult> DeleteCategory(int tagId)
        {
            var res = new AppResult();

            var tag = await _db.Categories.FirstOrDefaultAsync(x => x.Id == tagId);
            if (tag == null)
                return res.Bad("Category não encontrada.");

            _db.Categories.Remove(tag);
            await _db.SaveChangesAsync();

            return res.Good("Category removida com sucesso.");
        }

        //public async Task<AppResult> AddCategoryToItem(AddCategoryToItemDTO dto)
        //{
        //    var res = new AppResult();

        //    var tag = await _db.Categorys.FirstOrDefaultAsync(x => x.Id == dto.CategoryId);
        //    if (tag == null)
        //        return res.Bad("Category não encontrada.");

        //    var item = await _db.Items.FirstOrDefaultAsync(x => x.Id == dto.ItemId);
        //    if (item == null)
        //        return res.Bad("Item não encontrado.");

        //    // Assumindo que existe uma relação entre Categorys e Items
        //    item.Categorys.Add(tag);

        //    _db.Items.Update(item);
        //    await _db.SaveChangesAsync();

        //    return res.Good("Category adicionada ao item com sucesso.");
        //}

        public async Task<AppResult> GetAllCategorys()
        {
            var tags = await _db.Categories
                .Select(x => new CategoryDTO(x))
                .ToListAsync();
            return new AppResult().Good("Lista de tags", tags);
        }

        public async Task<AppResult> GetCategoryById(int id)
        {
            var res = new AppResult();

            // Busca a tag no banco de dados usando o ID
            var tag = await _db.Categories
                            .FirstOrDefaultAsync(t => t.Id == id);

            // Verifica se a tag existe
            if (tag == null)
                return res.Bad("Category não encontrada.");

            // Retorna a tag encontrada
            return res.Good("Category encontrada.", new CategoryDTO(tag));
        }

        public async Task<AppResult> GetActiveCategorys()
        {
            var activeCategorys = await _db.Categories
                .Where(x => x.IsActive)
                .Select(x => new CategoryDTO(x))
                .ToListAsync();
            return new AppResult().Good("Lista de tags ativas", activeCategorys);
        }
        public async Task<AppResult> DeassociateUserWithCategory(AssociateUserCategoryDTO dto)
        {
            var res = new AppResult();

            var user = await _db.Users.FindAsync(dto.UserId);
            var category = await _db.Categories.FindAsync(dto.CategoryId);

            if (user == null || category == null)
            {
                return res.Bad("Utilizador ou categoria inválida");
            }
            var userCategory = await _db.UserCategories.FirstOrDefaultAsync(uc => uc.UserId == dto.UserId && uc.CategoryId == dto.CategoryId);
            // Check if the association already exists
            if (userCategory != null)
            {
                return res.Bad("Utilizador não associado a categoria");
            }

            _db.UserCategories.Remove(userCategory);
            await _db.SaveChangesAsync();

            return res.Good("Associação feita com sucesso");
        }
        public async Task<AppResult> AssociateUserWithCategory(AssociateUserCategoryDTO dto)
        {
            var res = new AppResult();

            var user = await _db.Users.FindAsync(dto.UserId);
            var category = await _db.Categories.FindAsync(dto.CategoryId);

            if (user == null || category == null)
            {
                return res.Bad("Utilizador ou categoria inválida");
            }

            // Check if the association already exists
            if (await _db.UserCategories.AnyAsync(uc => uc.UserId == dto.UserId && uc.CategoryId == dto.CategoryId))
            {
                return res.Bad("Utilizador já associado a categoria");
            }

            var userCategory = new UserCategory
            {
                UserId = dto.UserId,
                CategoryId = dto.CategoryId
            };

            _db.UserCategories.Add(userCategory);
            await _db.SaveChangesAsync();

            return res.Good("Associação feita com sucesso");
        }
    }
}