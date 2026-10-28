using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class LecturersServices : ILecturersServices
    {
        private readonly KeelteKoolV2Context _context;
        public LecturersServices(KeelteKoolV2Context context)
        {
            _context = context;
        }
        public async Task<Lecturer> Create(LecturerDTO dto)
        {
            //check if not null
            if (dto == null)
            {
                return null;
            }
            //convert to domain-type
            Lecturer domain = new Lecturer();
            domain.Id = Guid.NewGuid();
            if (string.IsNullOrEmpty(dto.FirstName))
            {
                dto.FirstName = "Martin";
            }
            if (string.IsNullOrEmpty(dto.LastName))
            {
                dto.LastName = "Rossakov";
            }
            domain.FirstName = dto.FirstName;
            domain.LastName = dto.LastName;
            domain.Qualifications = dto.Qualifications;
            domain.UserID = dto.UserID;

            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.Lecturers.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Lecturer> Delete(Guid id)
        {
            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lecturer == null)
            {
                return null;
            }

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return lecturer;
        }


        public async Task<Lecturer> DetailAsync(Guid id)
        {
            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(x => x.Id == id);

            return lecturer;
        }

        public async Task<Lecturer> Update(LecturerDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (lecturer == null)
            {
                return null;
            }

            lecturer.FirstName = dto.FirstName;
            lecturer.LastName = dto.LastName;
            lecturer.Qualifications = dto.Qualifications;
            lecturer.UserID = dto.UserID;
            lecturer.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return lecturer;
        }

    }
}