using System;
using System.Threading.Tasks;

namespace BookStore.Web.Abstract
{
    /// <summary>
    /// This is an interface for a repository
    /// </summary>
    /// <typeparam name="TEntity">This is entity type for the repository</typeparam>
    public interface IRepository<TEntity> : IDisposable
    {
        /// <summary>
        /// This method adds an entity object into the repository
        /// </summary>
        /// <param name="entity">This is the entity</param>
        /// <returns>The entity added into the repository</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// This method edits an entity in the repository
        /// </summary>
        /// <param name="entity">This is a modified entity object</param>
        void Edit(TEntity entity);

        /// <summary>
        /// This method removes an entity from the repository
        /// </summary>
        /// <param name="entity">This is an entity that will be removed</param>
        /// <returns>The deleted entity</returns>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// This finds an entity in the repository by its id
        /// </summary>
        /// <param name="id">This is an entity's id</param>
        /// <returns>The found entity</returns>
        Task<TEntity> FindById(int id);

        /// <summary>
        /// This saves changes made on a repository
        /// </summary>
        /// <returns>A task carrying an integer</returns>
        Task<int> Save();
    }
}
