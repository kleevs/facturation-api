using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Db
{
    public class FakeDbSet<T> : DbSet<T>, IQueryable<T> where T : class
    {
        public Type ElementType => _query.ElementType;
        public Expression Expression => _query.Expression;
        public IQueryProvider Provider => _query.Provider;

        private IList<T> _data;
        private IQueryable<T> _query;

        public FakeDbSet() : base()
        {
            _data = new List<T>();
            _query = _data.AsQueryable();
        }

        public override EntityEntry<T> Add(T entity)
        {
            _data.Add(entity);
            return null;
        }

        public override void AddRange(IEnumerable<T> entity)
        {
            entity.Select(Add).ToList();
        }

        public override EntityEntry<T> Remove(T entity)
        {
            _data.Remove(entity);
            return null;
        }

        public IEnumerator<T> GetEnumerator() => 
            _query.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            _query.GetEnumerator();
    }
}
