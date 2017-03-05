using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest
{
    public interface IFluentHttp
    {
        IFluentOperation Create(string url);
    }
}
