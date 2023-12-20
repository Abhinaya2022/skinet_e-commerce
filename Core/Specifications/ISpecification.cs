using System.Linq.Expressions;

namespace Core.Specifications;

public interface ISpecification<T>
{
    /// <summary>
    /// Criteria applies for where expression to implement filter condition
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Includes is used to load relational entity eagarly
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }
   
    /// <summary>
    /// It will apply orderBy Condition 
    /// </summary>
    Expression<Func<T, object>> OrderBy { get; }
    
    /// <summary>
    /// It will apply orderByDescending condition 
    /// </summary>
    Expression<Func<T, object>> OrderByDescending { get; }

    int Take{ get; }

    int Skip { get; }

    bool IsPagingEnabled { get; }
}
