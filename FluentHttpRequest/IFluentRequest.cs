using FluentHttpRequest.CacheExtension;
using System;
using System.Collections;
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

        
        T Get<T>(string path = null);
        Task<T> GetAsync<T>(string path = null);
        Task<T> GetAsync<T>(CancellationToken cancellationToken, string path = null);

        
        T Post<T>(string path = null);
        Task<T> PostAsync<T>(string path = null);        
        Task<T> PostAsync<T>(CancellationToken cancellationToken, string path = null);
        
        string GetQueryString(bool printPort = false);
    }
}
