using System;

namespace AuditManager.Model.EFModel.AM
{
    public abstract class BaseEntity
    {

    }

    public abstract class PostFix : BaseEntity
    {
        public int ActorId { get; set; }
        public virtual User Actor { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }

    public abstract class ShortCommonProperty : BaseEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }

    public abstract class CommonProperty : BaseEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public int IsActive { get; set; }
    }
}
