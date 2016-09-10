using BookStore.Web.Concrete;
using System.Threading.Tasks;

namespace BookStore.Web.Abstract
{
    /// <summary>
    /// This is an abstract class for all repositories in this app
    /// </summary>
    /// <typeparam name="TEntity">This is entity type of a repository</typeparam>
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// This is a database context instance
        /// </summary>
        private StoreDbContext dbContext;

        /// <summary>
        /// This is a property for the database context
        /// </summary>
        public StoreDbContext DbContext { get { return dbContext; } }

        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        /// <param name="dbContext">This is a database context object</param>
        public AbstractRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// This adds an entity into a repository
        /// </summary>
        /// <param name="entity">This is the entity</param>
        /// <returns>The entity that is added into the repository</returns>
        public abstract TEntity Add(TEntity entity);

        /// <summary>
        /// This deletes an entity from a repository
        /// </summary>
        /// <param name="entity">This is the entity that will be removed</param>
        /// <returns>The deleted entity</returns>
        public abstract TEntity Delete(TEntity entity);

        /// <summary>
        /// This edits an entity in a repository
        /// </summary>
        /// <param name="entity">This is an edited entity</param>
        /// <returns>The edited entity</returns>
        public abstract TEntity Edit(TEntity entity);

        /// <summary>
        /// This finds an entity by its id
        /// </summary>
        /// <param name="id">This is an entity's id</param>
        /// <returns>A task that contains the found entity or null</returns>
        public abstract Task<TEntity> FindById(int id);

        /// <summary>
        /// This saves changes made on a repository
        /// </summary>
        /// <returns>A task that contains an integer</returns>
        public async Task<int> Save()
        {
            return await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// This releases database resource
        /// </summary>
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}