using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProgram.Database.Repository
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		DbSet<TEntity> Table { get; }
		void Create(TEntity item);
		TEntity FindById(int id);
		IEnumerable<TEntity> Get();
		IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
		void Remove(TEntity item);
		void Update(TEntity item);
		void SaveChanges();
		Task CreateAsync(TEntity entity);
	}
}
