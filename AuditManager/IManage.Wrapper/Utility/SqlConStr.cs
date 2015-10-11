using IM.Wrapper.Model;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace IM.Wrapper.Utility
{
    internal class SqlConStr
    {
        public static string GetSqlConStr(IMInstance iMInstance)
        {
            string providerName = "System.Data.SqlClient";
            
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = iMInstance.DataSource;
            sqlBuilder.InitialCatalog = iMInstance.InitialCatalog;
            sqlBuilder.IntegratedSecurity = iMInstance.IntegratedSecurity;

            sqlBuilder.UserID = iMInstance.SqlUserID;
            sqlBuilder.Password = iMInstance.SqlPassword;

            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            entityBuilder.Provider = providerName;

            entityBuilder.ProviderConnectionString = providerString;
            
            return sqlBuilder.ToString();
        }
    }
}
