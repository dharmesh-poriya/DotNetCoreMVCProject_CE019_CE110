using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;
        public SQLStudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        Student IStudentRepository.Add(Student student)
        {
            context.Student.Add(student);
            context.SaveChanges();
            return student;
        }

        Student IStudentRepository.GetById(int Id)
        {
            return context.Student.FirstOrDefault(m => m.Id == Id);
        }

        Student IStudentRepository.GetByUniversityId(string UniversityId)
        {
            return context.Student.FirstOrDefault(m => m.UniversityId == UniversityId);
        }
        Student IStudentRepository.GetByLoginId(int LoginId)
        {
            return context.Student.FirstOrDefault(m => m.LoginId == LoginId);
        }
        IEnumerable<Student> IStudentRepository.GetByBranchId(int BranchId)
        {
            return context.Student.Where(m => m.BranchId == BranchId);
        }
    }
}
