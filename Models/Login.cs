using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceManagementSystem.Models
{
    public class Login
    {
        public int Id { get;}
        [Required(ErrorMessage ="Please enter username"), StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be 2 to 50 cahracter long")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password"), StringLength(50)]
        public string Password { get; set; }

        [NotMapped]
        public bool remember { get; set; }
        public Admin admin { get; set; }
        public Student Student { get; set; }
        public Faculty Faculty { get; set; }
    }
}
