using Core.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }
        // الترتيب (Ordering)
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }
        // الترقيم (Paging)
        if (spec.IsPagingEnabled && spec.Skip.HasValue && spec.Take.HasValue)
        {
            query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);
        }
        return query;
    }

    public static IQueryable<T> GetQueryForCount(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }
        // لا نطبق الترتيب أو الترقيم للعد
        return query;
    }
}

public static class SpecificationEvaluator
{
    public static IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> spec) where T : class
    {
        var query = inputQuery;
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }
        if (spec.IsPagingEnabled && spec.Skip.HasValue && spec.Take.HasValue)
        {
            query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);
        }
        return query.Select(spec.Selector);
    }
}


