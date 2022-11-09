using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceManagementSystem.Models
{
    public class Subject
    {
        public int Id { get;}

        [Required]
        public string SubjectName { get; set; }

        [Required]
        public string SubjectCode { get; set; }
        [Required]
        public string SubjectDescription { get; set; }

        [Required]
        public int SemesterNo { get; set; }


        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public Faculty Faculty { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }
        
    }
}
