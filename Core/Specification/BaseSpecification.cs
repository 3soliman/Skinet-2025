using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

public class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Expression<Func<T, object>>? OrderBy { get; protected set; } // إضافة
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; } // إضافة
    public int? Skip { get; protected set; }
    public int? Take { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }

    public BaseSpecification() { }


    // دوال مساعدة لإضافة الترتيب
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}

public class BaseSpecification<T, TResult> : ISpecification<T, TResult>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Expression<Func<T, TResult>> Selector { get; protected set; }
    public Expression<Func<T, object>>? OrderBy { get; protected set; }
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
    public int? Skip { get; protected set; }
    public int? Take { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }

    public BaseSpecification(Expression<Func<T, TResult>> selector)
    {
        Selector = selector;
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria, Expression<Func<T, TResult>> selector)
    {
        Criteria = criteria;
        Selector = selector;
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}



