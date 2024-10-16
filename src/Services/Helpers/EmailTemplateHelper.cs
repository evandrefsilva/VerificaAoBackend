
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Helpers
{

    public static class EmailTemplateHelper
    {
        public static string BuildEmail(string templatePath, string templateChacheKey, object model)
        {
            string template = File.ReadAllText(templatePath);
            var result = Engine.Razor.RunCompile(template, templateChacheKey, null, model);
            return result;
        }
    }
}
