using System;

namespace DRP.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime F_CreatorTime { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual string F_CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntity()
        {
            F_CreatorTime = DateTime.Now;
        }
    }
}