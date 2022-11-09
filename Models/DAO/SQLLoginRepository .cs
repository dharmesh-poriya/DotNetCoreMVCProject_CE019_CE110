using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLLoginRepository : ILoginRepository
    {
        private readonly AppDbContext context;
        public SQLLoginRepository(AppDbContext context)
        {
            this.context = context;
        }
        Login ILoginRepository.Add(Login login)
        {
            context.Login.Add(login);
            context.SaveChanges();
            return login;
        }
        Login ILoginRepository.Update(Login loginChanges)
        {
            var login = context.Login.Attach(loginChanges);
            login.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return loginChanges;
        }
        Login ILoginRepository.Delete(int Id)
        {
            Login login = context.Login.Find(Id);
            if (null != login)
            {
                context.Login.Remove(login);
                context.SaveChanges();
            }
            return login;
        }
        Login ILoginRepository.GetLogin(string username, string password)
        {
            return context.Login.FirstOrDefault(m => m.Username == username && m.Password == password);
        }

        Login ILoginRepository.GetByUserName(string username)
        {
            return context.Login.FirstOrDefault(m => m.Username == username);
        }
    }
}
