using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Interfaces
{
    public interface ICategorized<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        List<TKey> ParentCategoryIds { get; set; }
        
        
    }
}
