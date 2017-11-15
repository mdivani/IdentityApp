using IdentityDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRepository.Repositories
{
    //Base CRUD operations with Generic repository
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected DbSet<TEntity> _set;

        internal BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //returns dbset with given entity
        protected DbSet<TEntity> Set
        {
            get { return _set ?? (_set = _context.Set<TEntity>()); }
        }

        //returns all entities
        public List<TEntity> GetAll()
        {
            return Set.ToList();
        }

        //returns all entities for async
        public Task<List<TEntity>> GetAllAsync()
        {
            return Set.ToListAsync();
        }

        //cancels operation
        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Set.ToListAsync(cancellationToken);
        }

        //returns entities from position till chosen position
        public List<TEntity> PageAll(int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToList();
        }
        //returns entities from position till chosen position for async
        public Task<List<TEntity>> PageAllAsync(int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToListAsync();
        }
        //cancels operation with correct token
        public Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return Set.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }
        //returns Entity by id, if not found returns null
        public TEntity FindById(object id)
        {
            return Set.Find(id);
        }

        public Task<TEntity> FindByIdAsync(object id)
        {
            return Set.FindAsync(id);
        }

        public Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            return Set.FindAsync(cancellationToken, id);
        }
        //adds new entity
        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }
        //updates existing entity, if entity doesn't exists adds new.
        public virtual void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified; //do it here
                Set.Attach(entity); //attach
            }
        }

        //removes existing entity
        public void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }
        //gets list of entities by expression, allows to sort and include virtual properties
        public virtual IEnumerable<TEntity> Get(
       Expression<Func<TEntity, bool>> filter = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
       string includeProperties = "")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}

