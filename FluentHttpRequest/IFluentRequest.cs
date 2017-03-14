using FluentHttpRequest.CacheExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentHttpRequest
{
    public interface IFluentOperation
    {
        IFluentOperation AddParam(string param, string value);
        IFluentOperation AddHeader(string header, string value);
        IFluentOperation AddBodyParam(string param, string value);
        IFluentOperation AddCertificate(string name, StoreName storeName = StoreName.Root, StoreLocation storeLocation = StoreLocation.CurrentUser);
        IFluentOperation AddCertificate(string name, string password);
        IFluentProcess Get();
        IFluentProcess Post();
        Task<IFluentProcess> GetAsync();
        Task<IFluentProcess> GetAsync(CancellationToken cancellationToken);
        Task<IFluentProcess> PostAsync();
        Task<IFluentProcess> PostAsync(CancellationToken cancellationToken);
        string GetQueryString(bool printPort = false);
    }

    public interface IFluentProcess : IFluentTransform
    {
        IFluentTransform Extract(string path);        
    }

    public interface IFluentTransform
    {
        T Fill<T>();
        T FillWithCache<T>(string key, string region, bool withFallback = false);        
    }
}
