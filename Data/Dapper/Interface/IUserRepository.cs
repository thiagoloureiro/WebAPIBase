using Model;

namespace Data.Dapper.Interface
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);

        void InsertUser(string username, string password);
    }
}