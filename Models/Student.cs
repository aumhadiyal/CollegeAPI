using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp.Models
{
    public class Student
    {/*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]*/
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? AdmissionDate { get; set; }
    }
}
