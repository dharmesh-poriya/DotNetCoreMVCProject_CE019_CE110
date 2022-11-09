namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface IAdminRepository
    {
        Admin GetAdmin(int Id);
        Admin GetAdminByLoginId(int LoginId);

    }
}
