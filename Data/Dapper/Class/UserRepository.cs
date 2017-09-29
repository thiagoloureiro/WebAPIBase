using Dapper;
using Data.Dapper.Interface;
using Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Dapper.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string username, string password)
        {
            User ret;
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"select Id, [Name], Surname, Email, Phone, LastLogon, CreatedOn, ActivationCode
                from [User] U
                where [Login] = @Login and [Password] = @Password";

                ret = db.Query<User>(sql, new { Login = username, Password = password }, commandType: CommandType.Text).FirstOrDefault();
            }

            return ret;
        }

        public void InsertUser(string username, string password)
        {
            using (var db = new SqlConnection(connstring))
            {
                const string sql = @"insert into [User] ([Login], [Password], [CreatedOn], [LastLogon]) values (@Login, @Password, GETDATE(), GETDATE())";

                db.Execute(sql, new { Login = username, Password = password }, commandType: CommandType.Text);
            }
        }
    }
}