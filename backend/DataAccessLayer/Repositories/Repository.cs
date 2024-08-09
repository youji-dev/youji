using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.DataAccess.Repositories
{
    public class Repository<TEntity, TId>
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public virtual Task<TEntity> GetAsync(TId id)
        {
        }

        public virtual TEntity FindAsync(Expression<Func<TEntity, bool>> expression)
        {
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
        }
    }
}
