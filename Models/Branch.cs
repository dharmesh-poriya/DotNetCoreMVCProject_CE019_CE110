using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentAttendanceManagementSystem.Models
{
    public class Branch
    {
        public int Id { get;}

        [Required]
        public string BranchName { get; set; }

        [Required]
        [Range(1,8)]
        public int TotalSemester { get; set; }
        
        public ICollection<Subject> Subjects { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
