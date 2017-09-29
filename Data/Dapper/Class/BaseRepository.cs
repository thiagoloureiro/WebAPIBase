using System.Configuration;

namespace Data.Dapper.Class
{
    public abstract class BaseRepository
    {
        public string connstring = ConfigurationManager.ConnectionStrings["SqlServerConnString"].ConnectionString;
    }
}