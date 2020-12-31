using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FileSystem
{
    public class FileQueryable<T> : IQueryable<T>, IQueryProvider
    {
        private readonly IQueryProvider _provider;
        private readonly Expression _expression;
        private readonly ParameterExpression _root;
        private readonly Func<Expression<Func<IQueryable<T>, IQueryable<T>>>, IQueryable<T>> _callback;

        private FileQueryable(Func<Expression<Func<IQueryable<T>, IQueryable<T>>>, IEnumerable<T>> callback)
        {
            _provider = this;
            _expression = _root = Expression.Parameter(typeof(IQueryable<T>), "root");
            _callback = (e) => callback(e).AsQueryable<T>();
        }

        private FileQueryable(IQueryProvider provider, Expression expression)
        {
            _provider = provider;
            _expression = expression;
        }

        Type IQueryable.ElementType => typeof(T);
        Expression IQueryable.Expression => _expression;
        IQueryProvider IQueryable.Provider => _provider;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _provider.Execute<IEnumerable<T>>(_expression).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _provider.Execute<IEnumerable>(_expression).GetEnumerator();

        object IQueryProvider.Execute(Expression expression) => (this as IQueryProvider).Execute<object>(expression);
        IQueryable IQueryProvider.CreateQuery(Expression expression) => (this as IQueryProvider).CreateQuery<object>(expression);

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression) => new FileQueryable<TElement>(_provider, expression);
        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            Expression<Func<IQueryable<T>, IQueryable<T>>> param = null;
            if (expression is MethodCallExpression)
            {
                var callExpression = expression as MethodCallExpression;
                param = Expression.Lambda<Func<IQueryable<T>, IQueryable<T>>>(callExpression, new ParameterExpression[] { _root });
            }

            var result = _callback(param);
            return result is TResult ? (TResult)result : default(TResult);
        }

        public static IQueryable<T> Create(Func<Expression<Func<IQueryable<T>, IQueryable<T>>>, IEnumerable<T>> callback) => new FileQueryable<T>(callback);
    }
}
