using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Domain.Repositories;

namespace Template.Domain
{
    public interface IUnitOfWork<TKey> : IDisposable where TKey : IEquatable<TKey>
    {
        ISampleEntityRepository<TKey> SampleEntityRepository { get;  }
    }
}
