using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Domain;
using Template.Domain.Repositories;

namespace Template.Data.Mock
{
    public class MockUnitOfWork<TKey> : IUnitOfWork<TKey> where TKey : IEquatable<TKey>
    {
        public ISampleEntityRepository<TKey> SampleEntityRepository { get; set; }
        
        public void Dispose()
        {
           
        }
    }
}
