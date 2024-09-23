﻿using System.Linq.Expressions;

namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// Represents the repository base class.
    /// </summary>
    /// <typeparam name="TEntity">Instance of <see cref="TEntity"/></typeparam>
    /// <typeparam name="TId">Instance of <see cref="TId"/></typeparam>
    public class Repository<TEntity, TId>(DataContext context)
        where TEntity : class
    {
        /// <summary>
        /// The context of data
        /// </summary>
        protected DataContext Context { get => context; }

        /// <summary>
        /// Gets the entity with the specific id.
        /// </summary>
        /// <param name="id">Instance of <see cref="TId"/></param>
        /// <returns>The entity with the specific id.</returns>
        public async virtual Task<TEntity?> GetAsync(TId id)
        {
            return await this.Context.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        public virtual TEntity[]? GetAllAsync()
        {
            return [.. this.Context.Set<TEntity>()];
        }

        /// <summary>
        /// Get all entities that matches the expression.
        /// </summary>
        /// <param name="expression">Instance of <see cref="Expression"/></param>
        /// <returns>All entities matches the expression.</returns>
        public virtual List<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Where(expression).ToList();
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/></param>
        /// <returns>Represents an asynchronous <see cref="Task"/> that returns the added entity.</returns>
        public async virtual Task AddAsync(TEntity entity)
        {
            await this.Context.Set<TEntity>().AddAsync(entity);
            await this.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/></param>
        /// <returns>Represents an asynchronous <see cref="Task"/> that does not return a value.</returns>
        public async virtual Task UpdateAsync(TEntity entity)
        {
            this.Context.Set<TEntity>().Update(entity);
            await this.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/></param>
        /// <returns>Represents an asynchronous <see cref="Task"/> that does not return a value.</returns>
        public async virtual Task DeleteAsync(TEntity entity)
        {
            this.Context.Set<TEntity>().Remove(entity);
            await this.Context.SaveChangesAsync();
        }
    }
}
