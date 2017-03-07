using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.LifecycleManagement
{
    public interface IFluentEnviroment
    {
        IFluentEndpoint Env(string enviroment);
    }

    public interface IFluentEndpoint
    {
        IFluentSecurity Endpoint(string endpoint);        
    }

    public interface IFluentSecurity : IFluentOperation
    {
        IFluentOperation AddSecurityKey(string key, string secret);
    }
}
