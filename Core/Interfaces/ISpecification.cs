using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
     Expression<Func<T, object>>? OrderBy { get; } // إضافة دعم للترتيب
    Expression<Func<T, object>>? OrderByDescending { get; } // إضافة دعم للترتيب التنازلي
    int? Skip { get; }
    int? Take { get; }
    bool IsPagingEnabled { get; }
}


public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>> Selector { get; }
}


