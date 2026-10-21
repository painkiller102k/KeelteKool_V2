namespace KeelteKoolV2.Models.Lecturers
{
    public class LecturerViewModel
    {
        public Guid? Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Qualifications { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}