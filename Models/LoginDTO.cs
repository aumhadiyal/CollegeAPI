using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{

    public class LoginDTO
    {
        [Required]
        public string username { get; set; }

        [Required]

        public string password { get; set; }
    }
}
