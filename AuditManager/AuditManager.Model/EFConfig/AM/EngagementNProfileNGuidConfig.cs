using AuditManager.Model.EFModel.AM;

namespace AuditManager.Model.EFConfig.AM
{
    public class EngagementConfig : BaseEntityConfig<Engagement>
    {
        public EngagementConfig()
        {
            ToTable("Engagement");

            HasKey(t => t.Id);
        }
    }

    public class EngagementProfileConfig : BaseEntityConfig<EngagementProfile>
    {
        public EngagementProfileConfig()
        {
            ToTable("EngagementProfile");
        }
    }

    public class EngagementGuidConfig : BaseEntityConfig<EngagementGuid>
    {
        public EngagementGuidConfig()
        {
            ToTable("EngagementGuid");
        }
    }
}
