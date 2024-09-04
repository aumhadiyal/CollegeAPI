using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        /*        [Required]
                public int Id { get; set; }
                [Required]*/
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Address { get; set; }

        public DateTime? AdmissionDate { get; set; }
    }


}
