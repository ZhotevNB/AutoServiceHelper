using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutoServiceHelper.Infrastructure.Data.Common
{
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public Repository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public void Add<T>(T entity) where T : class
        {
            DbSet<T>().Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            DbSet<T>().Remove(entity);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>().AsQueryable();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        private DbSet<T> DbSet<T>() where T : class 
        { 
            return dbContext.Set<T>(); 
        }
        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            this.DbSet<T>().RemoveRange(entities);
        }

    }
}
