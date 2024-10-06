using Data;
using Data.Context;
using Services.Models;
using Services.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public interface IAppSettingsService
    {
        public string EmailSystem();
        public string Email();
        public AppResult GetAll();
        public AppResult Edit(Dictionary<string, string> dto);
    }
    public class AppSettingsService : IAppSettingsService
    {
        DataContext _db;
        public AppSettingsService(DataContext db)
        {
            _db = db;
        }
        public AppResult Edit(Dictionary<string, string> dtos)
        {
            AppResult res = new AppResult();

            try
            {
                var configs = _db.AppConfigurations
                    .Where(x => dtos.Select(y => y.Key).Contains(x.Key)).ToList();
                foreach (var item in configs)
                {
                    var value = dtos.FirstOrDefault(x => item.Key.ToLower().Trim()==x.Key.ToLower().Trim())
                        .Value;
                    item.Value = value;
                    _db.AppConfigurations.Update(item);
                }
                _db.SaveChanges();
                return res.Good("Configurações actualizadas com sucesso");
            }
            catch (Exception)
            {
                return res.Bad("Erro ao actualizar configurações");
            }
        }

        public string Email()
        {
            throw new NotImplementedException();
        }

        public string EmailNcr()
        {
            return _db.AppConfigurations.FirstOrDefault(x => x.Key == "EmailSupervisorNcr")?.Value;
        }
        public string EmailSintese()
        {
            return _db.AppConfigurations.FirstOrDefault(x => x.Key == "EmailSupervisorSintese")?.Value;
        }

        public string EmailStylus()
        {
            return _db.AppConfigurations.FirstOrDefault(x => x.Key == "EmailSupervisorNcrStylus")?.Value;
        }

        public string EmailSystem()
        {
            return _db.AppConfigurations.FirstOrDefault(x => x.Key == "Email")?.Value;
        }

        public AppResult GetAll()
        {
            AppResult res = new AppResult();
            try
            {
                var list = _db.AppConfigurations
                    .ToDictionary(x => x.Key, x => x.Value);
                return res.Good(list);
            }
            catch (Exception)
            {
                return res.Bad("Erro ao consultar lista de configurações");
            }
        }
    }


}
