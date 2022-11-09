using System.Collections;
using System.Collections.Generic;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface IBranchRepository
    {
        Branch Add(Branch branch);
        Branch GetById(int id);
        IEnumerable<Branch> GetAllBranch();
    }
}
