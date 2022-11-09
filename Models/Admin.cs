using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceManagementSystem.Models
{
    public class Admin
    {
        public int Id { get;}

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int LoginId { get; set; }
        public Login Login { get; set; }
    }
}
