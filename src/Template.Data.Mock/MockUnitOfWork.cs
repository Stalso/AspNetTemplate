using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Domain;
using Template.Domain.Repositories;
using Template.Data.Mock.Repositories;
using Microsoft.Data.Entity;

namespace Template.Data.Mock
{
    public class MockUnitOfWork<TKey,TContext> : IUnitOfWork<TKey> where TKey : IEquatable<TKey> where TContext : DbContext
    {
        #region Fields
        private TContext _context;
        private ISampleEntityRepository<TKey> _sampleEntityRepository;

        #endregion
        public MockUnitOfWork(TContext context)
        {
            _context = context;
        }
        public ISampleEntityRepository<TKey> SampleEntityRepository 
        {
            get { return _sampleEntityRepository ?? (_sampleEntityRepository = new SampleEntityRepository<TKey>(_context)); }
        }

        public void Dispose()
        {
            _sampleEntityRepository = null;
            _context.Dispose();
        }
    }
}
