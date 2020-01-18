using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
	public class SQLRepository<T> : IRepository<T> where T : BaseEntity
	{
		internal DataContext dataContext;
		internal DbSet<T> dbSet;

		public SQLRepository(DataContext dataContext)
		{
			this.dataContext = dataContext;
			this.dbSet = dataContext.Set<T>();
		}
		public IQueryable<T> Collection()
		{
			return dbSet;
		}

		public void Commit()
		{
			dataContext.SaveChanges();
		}

		public void Delete(string Id)
		{
			var t = Find(Id);

			if(dataContext.Entry(t).State == EntityState.Detached)
			{
				dbSet.Attach(t);				
			}

			dbSet.Remove(t);
		}

		public T Find(string Id)
		{
			return dbSet.Find(Id);
		}

		public void Insert(T p)
		{
			dbSet.Add(p);
		}

		public void Update(T t)
		{
			dbSet.Attach(t);
			dataContext.Entry(t).State = EntityState.Modified;
		}
	}
}
