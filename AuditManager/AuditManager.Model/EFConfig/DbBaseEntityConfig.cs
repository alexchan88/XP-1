using AuditManager.Model.EFModel;
using AuditManager.Model.EFModel.AM;
using System.Data.Entity.ModelConfiguration;

namespace AuditManager.Model.EFConfig
{
    public abstract class DbBaseEntityConfig<T> : EntityTypeConfiguration<T>
        where T : AmDbEntityModel
    {
        protected DbBaseEntityConfig() { }
    }

    public abstract class DbBaseComplexConfig<T> : ComplexTypeConfiguration<T>
        where T : AmDbEntityModel
    {
        protected DbBaseComplexConfig() { }
    }

    public abstract class BaseEntityConfig<T> : EntityTypeConfiguration<T>
        where T : BaseEntity
    {
        protected BaseEntityConfig() { }
    }
}
