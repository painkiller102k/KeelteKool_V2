using System.ComponentModel.DataAnnotations;

namespace KeelteKoolV2.Models.Lecturers
{
    public class LecturerCreateUpdateViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Eesnimi on kohustuslik.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Perekonnanimi on kohustuslik.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kvalifikatsioon on kohustuslik.")]
        [StringLength(200)]
        public string Qualifications { get; set; } = string.Empty;

        public string? UserID { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}