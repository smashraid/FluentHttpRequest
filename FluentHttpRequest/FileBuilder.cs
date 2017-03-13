using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHttpRequest.FileExtension
{
    public class FileBuilder : IFluentFileFtp, IFluentFileSFtp
    {
        private string _host;
        private string _username;
        private string _password;

        private FileBuilder()
        {

        }

        public static FileBuilder Ftp
        {
            get
            {
                return new FileBuilder();
            }
        }

        public static FileBuilder Sftp
        {
            get
            {
                return new FileBuilder();
            }
        }

        public static FileBuilder Flat
        {
            get
            {
                return new FileBuilder();
            }
        }

        public byte[] Download()
        {
            throw new NotImplementedException();
        }

        public void Upload()
        {
            throw new NotImplementedException();
        }

        public void Write(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public async void WriteAsync(string path, string contents)
        {
            
            byte[] result = Encoding.ASCII.GetBytes(contents);
            using (FileStream sourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(result, 0, result.Length);
            }
        }

        public async Task<string> ReadAsync(string path)
        {
            byte[] result;
            string text = string.Empty;
            using (FileStream sourceStream = File.Open(path, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
                text = Encoding.ASCII.GetString(result);
            }
            return text;
        }

        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
