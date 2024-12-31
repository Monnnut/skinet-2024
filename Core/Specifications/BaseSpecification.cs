using System;
using System.Dynamic;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
  //allows you to define a flitering condition using an expression
  //stored condition in Criteria to make it accessible later on

  //without below, there will be an error cause the param required an expression
  //serves to initialize the base class with null criteria
  //allowing the creation of instances of BaseSpecification without 
  //requiring specific filtering criteria when they are not needed.
  protected BaseSpecification() : this(null) { }
  public Expression<Func<T, bool>>? Criteria => criteria;

  public Expression<Func<T, object>>? OrderBy { get; private set; }

  public Expression<Func<T, object>>? OrderByDescending { get; private set; }

  public bool IsDistinct { get; private set; }

  public int Take { get; private set; }

  public int Skip { get; private set; }

  public bool IsPagingEnabled { get; private set; }

  public IQueryable<T> ApplyCriteria(IQueryable<T> query)
  {
    if (Criteria != null)
    {
      query = query.Where(Criteria);

    }
    return query;
  }

  protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
  {
    OrderBy = orderByExpression;
  }

  protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
  {
    OrderByDescending = orderByDescExpression;
  }

  protected void ApplyDistinct()
  {
    IsDistinct = true;
  }

  protected void ApplyPaging(int skip, int take)
  {
    Skip = skip;
    Take = take;
    IsPagingEnabled = true;
  }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
  protected BaseSpecification() : this(null) { }
  public Expression<Func<T, TResult>>? Select { get; private set; }
  protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
  {
    Select = selectExpression;
  }

}
