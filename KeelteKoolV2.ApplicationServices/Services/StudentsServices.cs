using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
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

        public async Task<Student?> Create(StudentDTO? dto)
        {
            if (dto == null)
            {
                return null;
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? "Student" : dto.FirstName,
                LastName = string.IsNullOrWhiteSpace(dto.LastName) ? "Unknown" : dto.LastName,
                UserID = dto.UserID,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> DetailAsync(Guid id)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student?> Update(StudentDTO? dto)
        {
            if (dto == null)
            {
                return null;
            }

            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (student == null)
            {
                return null;
            }

            student.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? student.FirstName : dto.FirstName;
            student.LastName = string.IsNullOrWhiteSpace(dto.LastName) ? student.LastName : dto.LastName;
            student.UserID = dto.UserID;
            student.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> Delete(Guid id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return null;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student;
        }
    }
}
