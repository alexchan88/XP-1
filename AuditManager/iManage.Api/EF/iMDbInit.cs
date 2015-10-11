using System.Data.Entity;

namespace iManage.Api
{
    internal class iMDbInit : CreateDatabaseIfNotExists<iMDbContext>
    {
    }
}
