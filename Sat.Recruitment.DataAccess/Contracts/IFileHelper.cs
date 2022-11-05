using System.IO;

namespace Sat.Recruitment.DataAccess.Contracts
{
    public interface IFileHelper
    {
        MemoryStream GetFileData(string path);
        void SaveFileData(string path, MemoryStream ms);
    }
}