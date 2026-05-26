using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	protected RepositoryContext RepositoryContext;

	protected RepositoryBase(RepositoryContext repositoryContext)
		=> RepositoryContext = repositoryContext;
	
	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
	bool trackChanges) =>
		!trackChanges ?
		  RepositoryContext.Set<T>()
			.Where(expression)
			.AsNoTracking() :
		  RepositoryContext.Set<T>()
			.Where(expression);

	public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
}