using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace StudentAttendanceManagementSystem.Models
{
    public class StudentSubject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Attendance { get; set; }

        [Required]
        public int SemesterNo { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
        
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
