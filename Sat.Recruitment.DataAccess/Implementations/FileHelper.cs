using System;
using System.IO;
using Sat.Recruitment.DataAccess.Contracts;

namespace Sat.Recruitment.DataAccess.Implementations
{
    public class FileHelper : IFileHelper
    {
        public MemoryStream GetFileData(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.Read))
            {
                file.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }
            return ms;
        }

        public void SaveFileData(string path, MemoryStream ms)
        {
            using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }
    }
}
