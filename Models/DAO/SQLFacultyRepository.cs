using System.Collections.Generic;
using System.Linq;
namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLFacultyRepository : IFacultyRepository
    {
        private readonly AppDbContext context;
        public SQLFacultyRepository(AppDbContext context)
        {
            this.context = context;
        }
        Faculty IFacultyRepository.GetById(int Id)
        {
            return context.Faculty.FirstOrDefault(m => m.Id == Id);
        }

        Faculty IFacultyRepository.GetByLoginId(int loginId)
        {
            return context.Faculty.FirstOrDefault(m => m.LoginId == loginId);
        }

        IEnumerable<Faculty> IFacultyRepository.GetByBranchId(int branchId)
        {
            return context.Faculty.Where(m => m.BranchId == branchId);
        }

        IEnumerable<Faculty> IFacultyRepository.GetAll(int branchId)
        {
            return context.Faculty.Where(m => m.BranchId==branchId);
        }

        Faculty IFacultyRepository.GetBySubjectId(int subjectId)
        {
            return context.Faculty.FirstOrDefault(m => m.SubjectId == subjectId);
        }

        Faculty IFacultyRepository.GetByEmail(string email)
        {
            return context.Faculty.FirstOrDefault(m => m.Email == email);
        }

        Faculty IFacultyRepository.Add(Faculty newFaculty)
        {
            context.Faculty.Add(newFaculty);
            context.SaveChanges();
            return newFaculty;
        }
    }
}
