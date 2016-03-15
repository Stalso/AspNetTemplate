using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Interfaces
{
    /// <summary>
    /// Generic Entity interface.
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>        
      
        TKey Id { get; set; }
        long Version { get; set; }

        DateTime CreationDate { get; set; }

        DateTime LastUpdated { get; set; }
    }
    
}
