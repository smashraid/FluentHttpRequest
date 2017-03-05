using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest
{
    public interface IFluentOperation
    {
        IFluentOperation AddParam(string param, string value);
        IFluentOperation AddHeader(string header, string value);
        IFluentOperation AddBodyParam(string param, string value);

        IFluentProcess Get();
        IFluentProcess Post();
        Task<IFluentProcess> GetAsync();
        Task<IFluentProcess> PostAsync();

        string GetQueryString(bool printPort = false);
    }
}
