using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.LifecycleManagement
{
    public partial interface IFluentEnviroment
    {
        IFluentEndpoint Env(string enviroment);
    }

    public partial interface IFluentEndpoint
    {
        IFluentOperation Endpoint(string endpoint);
    }
}
