using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLSubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext context;
        public SQLSubjectRepository(AppDbContext context)
        {
            this.context = context;
        }

        Subject ISubjectRepository.Add(Subject newSubject)
        {
            context.Subject.Add(newSubject);
            context.SaveChanges();
            return newSubject;
        }

        Subject ISubjectRepository.GetById(int id)
        {
            return context.Subject.FirstOrDefault(m => m.Id == id);
        }

        Subject ISubjectRepository.GetBySubjectCode(string code)
        {
            return context.Subject.FirstOrDefault(m => m.SubjectCode == code);
        }

        IEnumerable<Subject> ISubjectRepository.GetAllByBranchId(int BranchId)
        {
            return context.Subject.Where(m => m.BranchId == BranchId).OrderBy(m => m.SemesterNo);
        }

        IEnumerable<Subject> ISubjectRepository.GetAllBySemesterBranchId(int SemesterNo,int BranchId)
        {
            return context.Subject.Where(m => (m.SemesterNo==SemesterNo && m.BranchId == BranchId));
        }
    }
}
