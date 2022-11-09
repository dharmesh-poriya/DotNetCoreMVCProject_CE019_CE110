using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLAdminRepository : IAdminRepository
    {
        private readonly AppDbContext context;
        public SQLAdminRepository(AppDbContext context)
        {
            this.context = context;
        }

        Admin IAdminRepository.GetAdmin(int Id)
        {
            return context.Admin.FirstOrDefault(m => m.Id == Id);
        }
        Admin IAdminRepository.GetAdminByLoginId(int LoginId)
        {
            return context.Admin.FirstOrDefault(m => m.LoginId == LoginId);
        }
    }
}
