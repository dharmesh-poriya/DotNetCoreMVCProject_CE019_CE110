using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;

namespace StudentAttendanceManagementSystem.Models
{
    public class Student
    {
        public int Id { get;}

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UniversityId { get; set; }

        public string ActualDOB { get; set; }
        public DateTime DOB { get; set; }

        public string Password { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int LoginId { get; set; }
        public Login Login { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
