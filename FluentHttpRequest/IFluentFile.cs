using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.FileExtension
{
    public interface IFluentFileFtp
    {
        void Upload();
        byte[] Download();

    }

    public interface IFluentFileSFtp
    {
        void Upload();
        byte[] Download();
    }
}
