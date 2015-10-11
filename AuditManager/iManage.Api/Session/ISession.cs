
namespace iManage.Api
{
    interface ISession
    {
        IManage.IManSession Session();
        IManage.IManDatabase DB(DbNameType dbNameType);
    }
}
