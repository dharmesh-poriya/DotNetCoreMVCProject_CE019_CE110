using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceManagementSystem.Models
{
    public class Faculty
    {
        public int Id { get;}

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Email { get; set; }

        public string Password { get; set; }
        
        public int LoginId { get; set; }
        public Login Login { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
