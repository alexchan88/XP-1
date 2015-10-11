
namespace AuditManager.Model.EFModel.AM
{
    public class WsActivity : AmDbEntityModel
    {
        public WsActivityType WsActivityTypeId { get; set; }
        public string Description { get; set; }
    }

    public class WsMailStatus : AmDbEntityModel
    {
        public WsMailStatusType WsMailStatusId { get; set; }
        public string Description { get; set; }
    }
}
