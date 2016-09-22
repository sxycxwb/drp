using System;

namespace DRP.Domain.Entities.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool F_DeleteMark { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual string F_DeleteUserId { get; set; }
        
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? F_DeleteTime { get; set; }
    }
}
