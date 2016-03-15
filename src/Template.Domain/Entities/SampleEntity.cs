using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Entities
{
    public class SampleEntity<TKey> : Entity<TKey> where TKey : IEquatable<TKey>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DbData { get; set; }
    }
}
