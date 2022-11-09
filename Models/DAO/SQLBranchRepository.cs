using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLBranchRepository : IBranchRepository
    {
        private readonly AppDbContext context;
        public SQLBranchRepository(AppDbContext context)
        {
            this.context = context;
        }

        Branch IBranchRepository.Add(Branch newBranch)
        {
            context.Branch.Add(newBranch);
            context.SaveChanges();
            return newBranch;
        }
        IEnumerable<Branch> IBranchRepository.GetAllBranch()
        {
            return context.Branch;
        }
        Branch IBranchRepository.GetById(int id)
        {
            return context.Branch.FirstOrDefault(m => m.Id == id);
        }
    }
}
