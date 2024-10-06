using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Services.Helpers
{
    public static class DtoMapperExtension
    {
        public static T MapTo<T>(this object value) =>
            JsonSerializer.Deserialize<T>(
                JsonSerializer.Serialize(value)
            );
    }
}
