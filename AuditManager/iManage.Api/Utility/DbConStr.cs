using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace iManage.Api
{
    public class DbConStr
    {
        public static string Get(EnvType envType)
        {
            return Get("useomcdp042\\Interwoven1", DbNameType.Active, false, "idms", "Interwoven2008");
        }

        public static string Get(string dataSource
            , DbNameType dbNameType = DbNameType.Active, bool integratedSecurity = true, string userID = null, string password = null)
        {
            string providerName = "System.Data.SqlClient";

            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = dataSource;
            sqlBuilder.InitialCatalog = dbNameType.ToString();

            sqlBuilder.IntegratedSecurity = integratedSecurity;

            if (!sqlBuilder.IntegratedSecurity)
            {
                sqlBuilder.UserID = userID;
                sqlBuilder.Password = password;
            }

            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            entityBuilder.Provider = providerName;

            entityBuilder.ProviderConnectionString = providerString;

            return sqlBuilder.ToString();
        }
    }
}
