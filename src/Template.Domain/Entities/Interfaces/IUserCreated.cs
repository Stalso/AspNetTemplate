using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities.Interfaces
{
    public interface IUserCreated<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey AuthorId { get; set; }
    }
}
