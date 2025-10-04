using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subfolder);
        void DeleteFile(string filePath);
    }
}