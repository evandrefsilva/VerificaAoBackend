using Data.Entities;
using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class AppAppSettingsDTO
    {
        public AppAppSettingsDTO() { }
        public AppAppSettingsDTO(AppSettings appAppSettings)
        {
            if(appAppSettings!=null)
            {
                Key = appAppSettings.Key;
                Value = appAppSettings.Value;
            }
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
