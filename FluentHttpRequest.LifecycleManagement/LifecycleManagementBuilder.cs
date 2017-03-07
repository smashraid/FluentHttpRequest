using FluentHttpRequest.LifecycleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest
{
    public partial class LifecycleManagementBuilder : IFluentEnviroment, IFluentEndpoint
    {
        private string _project;
        private string _enviroment;
        private string _endpoint;
        private Uri _uri;

        public RequestBuilder1()
        {

        }
        public RequestBuilder1(string name)
        {
            _project = name;
        }

        public static IFluentEnviroment Project(string name)
        {
            return new RequestBuilder1(name);
        }

        public IFluentEndpoint Env(string enviroment)
        {
            _enviroment = enviroment;
            return this;
        }

        public IFluentOperation Endpoint(string endpoint)
        {
            _endpoint = endpoint;
            string u = $"https://lm.cignium.com/run/cignium/{_project}/{_enviroment}/{_endpoint}";
            return new FluentHttpRequest.RequestBuilder() { _uri = new Uri(u) };            
        }
    }
}
