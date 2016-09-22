using System;

namespace DRP.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited
    {
        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? F_LastModifyTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual string F_LastModifyUserId { get; set; }
    }
}