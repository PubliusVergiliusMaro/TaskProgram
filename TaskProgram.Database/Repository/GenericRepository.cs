using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProgram.Database.Repository
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		DbContext _context;
		public DbSet<TEntity> Table
		{
			get
			{
				return _context.Set<TEntity>();
			}
		}
		public GenericRepository(DbContext context)
		{
			_context = context;
		}

		public IEnumerable<TEntity> Get()
		{
			return Table.AsNoTracking().ToList();
		}

		public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
		{
			return Table.AsNoTracking().Where(predicate).ToList();
		}
		public TEntity FindById(int id)
		{
			return Table.Find(id);
		}

		public void Create(TEntity item)
		{
			Table.Add(item);
			_context.SaveChanges();
		}
		public void Update(TEntity item)
		{
			_context.Entry(item).State = EntityState.Modified;
			_context.SaveChanges();
		}
		public void Remove(TEntity item)
		{
			Table.Remove(item);
			_context.SaveChanges();
		}

		public async void SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

		public async Task CreateAsync(TEntity entity)
		{
			await _context.Set<TEntity>().AddAsync(entity);
			await _context.SaveChangesAsync();
		}
	}
}
