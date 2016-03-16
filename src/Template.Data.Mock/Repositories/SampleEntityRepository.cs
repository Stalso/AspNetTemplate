
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Domain.Entities;
using Template.Domain.Repositories;

namespace Template.Data.Mock.Repositories
{
    public class SampleEntityRepository<TKey> : Repository<SampleEntity<TKey>, TKey>, ISampleEntityRepository<TKey> where TKey : IEquatable<TKey>
    {
        public SampleEntityRepository(DbContext context) : base(context)
        {
            
        }
    }
}
