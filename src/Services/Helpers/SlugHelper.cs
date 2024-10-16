using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = RemoveAccents(phrase).ToLower();
            // Remove caracteres inválidos
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // Remove espaços em branco extras
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // Substitui espaços por hífens
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        private static string RemoveAccents(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in text)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }

}
