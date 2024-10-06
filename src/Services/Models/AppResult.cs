using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class AppResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "Erro ao realizar operação";

        public object Object { get; set; }

        public AppResult Good(string msg, object obj = null)
        {
            Success = true;
            Message = msg;
            if (obj != null)
            {
                Object = obj;
            }
            return this;
        }

        public AppResult Bad(string msg, object obj = null)
        {
            Success = false;
            Message = msg;
            if (obj != null)
            {
                Object = obj;
            }
            return this;
        } 
        public AppResult Bad()
        {
            Success = false;
            Message = "Erro ao realizar operação";
            return this;
        }

        public AppResult Good(object obj)
        {
            Success = true;
            Message = "Operação realizada com exito";
            if (obj != null)
            {
                Object = obj;
            }
            return this;
        }
        public AppResult Good()
        {
            Success = true;
            Message = "Operação realizada com exito";
            Object = null;
            return this;
        }
        public AppResult Bad(object obj)
        {
            Success = false;
            if (obj != null)
            {
                Object = obj;
            }
            return this;
        }

    }
}
