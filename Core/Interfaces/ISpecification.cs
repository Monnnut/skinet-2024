using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    //Func<T,bool> delegate that represents a function taking a parm T and return bool
    //Adding Expression turns in expression tree
    //instead of compiled and execute directly, represent as data structure
    //that can be inspected, translated or execute later
    Expression<Func<T, bool>>? Criteria { get; }
    //for filtering in asc order
    //use object to represent all class
    Expression<Func<T, object>>? OrderBy { get; }
    //for filtering in desc order
    Expression<Func<T, object>>? OrderByDescending { get; }

    bool IsDistinct { get; }

}

//specification that returns another type

public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}
