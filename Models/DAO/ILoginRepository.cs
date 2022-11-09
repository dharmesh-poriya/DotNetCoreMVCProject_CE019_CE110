namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface ILoginRepository
    {
        Login GetLogin(string username, string password);
        Login GetByUserName(string username);
        Login Add(Login login);
        Login Update(Login login);
        Login Delete(int Id);

    }
}
