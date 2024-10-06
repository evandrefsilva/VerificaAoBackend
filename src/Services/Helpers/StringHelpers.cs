using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Services.Helpers
{
    public static class StringHelpers
    {

        public static string ToJson(this object o)
        {
            var ret = JsonConvert.SerializeObject(o,
                formatting: Formatting.Indented,
                new JsonSerializerSettings
                {
                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                });
            return string.IsNullOrEmpty(ret) ? "[]" : ret;
        }
        public static string ToPhoneNumber(this string phone)
        {
            try
            {
                if ( ! string.IsNullOrWhiteSpace(phone) && phone.Contains("+"))
                {
                    phone.Replace("+", "");
                    return double.Parse(phone.Replace(" ", "")).ToString("+### ### ### ###");
                }
                else return phone;

            }
            catch (Exception)
            {
                return phone;
            }


        }
        public static string ExtractFormattedNumber(this string input)
        {
            // Verifica se a string de entrada é nula ou vazia
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("A string de entrada não pode ser nula ou vazia.", nameof(input));
            }

            // Verifica se a string tem pelo menos 9 caracteres
            if (input.Length >= 9)
            {
                // Extrai os últimos 9 dígitos da string
                string lastNineDigits = input.Substring(input.Length - 9);

                // Formata os últimos 9 dígitos conforme necessário
                return lastNineDigits; // Altere esta parte para aplicar a formatação desejada
            }
            else
            {
                return input;
            }
        }
        public static string Truncate(this string str, int length = 150)
        {
            return str.Length > length ? str.Substring(0, length)+"..." : str; 
        }

        public static string GetDescription<T>(this T enumerationValue)
    where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
