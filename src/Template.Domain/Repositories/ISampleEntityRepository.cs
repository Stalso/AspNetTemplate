using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Domain.Repositories
{
    public interface ISampleEntityRepository<TKey>: IRepository<SampleEntity<TKey>, TKey> where TKey:IEquatable<TKey>
    {

    }
}
