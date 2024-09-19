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
        public string? NameDTO { get; set; }
        [EmailAddress]
        [MaxLength(50)]
        public string? EmailDTO { get; set; }
        [Required]
        [MaxLength(200)]
        public string? AddressDTO { get; set; }

        public DateTime? AdmissionDateDTO { get; set; }
    }


}
