using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class StudentsServices : IStudentsServices
    {
        private readonly KeelteKoolV2Context _context;

        public StudentsServices(KeelteKoolV2Context context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAll()
        {
            var result = await _context.Students
                .Take(20)
                .ToListAsync();

            return result;
        }

        public async Task<Student> DetailAsync(string userId)
        {
            var result = await _context.Students
                .FirstOrDefaultAsync(x => x.UserID == userId);

            return result;
        }
    }
}