using KeelteKoolV2.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeelteKoolV2.Core.ServiceInterface
{
    public interface IStudentsServices
    {
        Task<List<Student>> GetAll();

        Task<Student> DetailAsync(string userId);
    }
}