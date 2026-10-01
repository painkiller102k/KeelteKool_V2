using KeelteKoolV2.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeelteKoolV2.Data
{
    public class KeelteKoolV2Context : IdentityDbContext<ApplicationUser>
    {
        public KeelteKoolV2Context(DbContextOptions<KeelteKoolV2Context> options):base (options) 
        {
        }
            //tables set here
            public DbSet<LanguageCourse> LanguageCourses { get; set; }
    }
}
