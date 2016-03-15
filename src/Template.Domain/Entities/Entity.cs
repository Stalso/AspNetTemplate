
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities
{
    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>    
    public abstract class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
      
        public virtual TKey Id { get; set; }
        public long Version { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
    public  class CategorizedEntity<TKey> : Entity<TKey>, ICategorized<TKey> where TKey : IEquatable<TKey>
    {
        public List<TKey> ParentCategoryIds { get; set; }      
        public CategorizedEntity()
        {
            ParentCategoryIds = new List<TKey>();
        }

    }
}
