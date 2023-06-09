using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Domain;
using Microsoft.EntityFrameworkCore.Internal;

namespace Repository
{    
    public abstract class BaseRepository<DbEntity> where DbEntity : class, ITable, new()
    {
        IDbContextFactory<AppDbContext> DbContextFactory { get; set; }
        protected ITenantResolver TenantResolver { get; set; }
        public BaseRepository(IDbContextFactory<AppDbContext> dbContextFactory,ITenantResolver tenantResolver)
        {
            DbContextFactory = dbContextFactory;
            TenantResolver = tenantResolver;
        }
        protected bool LazyLoading { get; set; } = true;
        protected bool RelationsEagerLoading { get; set; } = true;
        protected async virtual Task<DbEntity> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _Get(p => p.Id == id);
        }
        protected async Task<int> Last()
        {
            using (DbContext db = await GetDbContext())
            {
                var record = db.Set<DbEntity>()
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefault();
                return record != null ? record.Id : 0;
            }
        }
        protected async Task<int> Max(Expression<Func<DbEntity, int>> selector)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Max(selector);
            }
        }
        protected async Task<int> Max(Expression<Func<DbEntity, int>> selector, Expression<Func<DbEntity, bool>> where)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(where)
                    .Select(selector)
                    .DefaultIfEmpty(0)
                    .Max();

            }
        }
        protected async Task<int> Count(Expression<Func<DbEntity, bool>> where)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Count(where);
            }
        }
        protected async Task<TResult> Get<TResult>(int id, Expression<Func<DbEntity, TResult>> selector)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(p => p.Id == id)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        protected async Task<TResult> Get<TResult>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(where)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        
        protected async Task<object> Get(int id, Expression<Func<DbEntity, object>> selector)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(p => p.Id == id)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        protected async Task<DbEntity> Get(Expression<Func<DbEntity, bool>> where)
        {
            return await _Get(where);
        }
        protected async Task<TResult> GetLast<TResult, TKey>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, TKey>> orderByDescending)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .OrderByDescending(orderByDescending)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        protected async Task<TResult> GetLast<TResult, TKey>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where, Expression<Func<DbEntity, TKey>> orderByDescending)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .OrderByDescending(orderByDescending)
                    .Where(where)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        protected async Task<TResult> GetFirst<TResult, TKey>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where, Expression<Func<DbEntity, TKey>> orderBy)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .OrderBy(orderBy)
                    .Where(where)
                    .Select(selector)
                    .FirstOrDefault();
            }
        }
        protected async Task<List<TResult>> GetLast<TResult, TKey>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where, Expression<Func<DbEntity, TKey>> orderByDescending, int numberOfRows)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .OrderByDescending(orderByDescending)
                    .Where(where)
                    .Select(selector)
                    .Take(numberOfRows)
                    .ToList();
            }
        }
        protected async Task<List<DbEntity>> GetAll()
        {
            List<DbEntity> entities = new List<DbEntity>();
            using (DbContext db = await GetDbContext())
            {
                entities = GetDbSet(db).ToList();
            }
            return entities;
        }
        protected async Task<List<TResult>> GetAll<TResult>(Expression<Func<DbEntity, TResult>> selector)
        {
            List<DbEntity> entities = new List<DbEntity>();
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Select(selector)
                    .ToList();
            }
        }
        protected async Task<List<DbEntity>> GetAllWithCriteria(Expression<Func<DbEntity, bool>> where)
        {
            List<DbEntity> entities = new List<DbEntity>();
            using (DbContext db = await GetDbContext())
            {
                entities = GetDbSet(db).AsQueryable().Where(where).ToList();
            }
            //if (JsonParserInfo.ParseJsonProperties)
            //    entities.ForEach(p => ParseEntityWithJsonProperties(p));
            return entities;
        }
        protected async Task<List<TResult>> GetAllWithCriteria<TResult>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(where)
                    .Select(selector)
                    .ToList();
            }
        }
        protected async Task<List<TResult>> GetAllWithCriteria<TResult>(Expression<Func<DbEntity, TResult>> selector, Expression<Func<DbEntity, bool>> where, Expression<Func<DbEntity, long>> orderBy)
        {
            using (DbContext db = await GetDbContext())
            {
                return db.Set<DbEntity>()
                    .Where(where)
                    .OrderBy(orderBy)
                    .Select(selector)
                    .ToList();
            }
        }
        protected async Task<int> Update(int? id, Action<DbEntity> editValues, bool createIfNotFound = false)
        {
            return await Update(p => p.Id == id, editValues, createIfNotFound);
        }
        protected async Task<int> Update(Expression<Func<DbEntity, bool>> where, Action<DbEntity> editValues, bool createIfNotFound = false, bool errorIfNotFound = true)
        {
            DbEntity dbEntity = default(DbEntity);
            using (DbContext db = await GetDbContext())
            {
                dbEntity = db.Set<DbEntity>()
                                        .Where(where)
                                        .FirstOrDefault();
                if (dbEntity == null)
                {
                    if (!createIfNotFound)
                    {
                        if (errorIfNotFound)
                            throw new Exception("Record not found to update!Error 909823498432092834");
                        else
                            return 0;
                    }
                    else
                        dbEntity = AddNew(db);
                }                
                editValues(dbEntity);
                if (dbEntity.GetType().IsSubclassOf(typeof(Tenant)))
                    (dbEntity as Tenant).TenantId = TenantResolver.GetCurrentTenantId();
                OnBeforeSave(dbEntity, db);
                db.SaveChanges();
                OnAfterSave(dbEntity, db);
            }
            return dbEntity.Id;
        }
        protected async Task<List<int>> UpdateMany(Action<DbEntity> editValues, Expression<Func<DbEntity, bool>> where = null)
        {
            List<DbEntity> dbEntities = null;
            using (DbContext db = await GetDbContext())
            {
                if (where != null)
                    dbEntities = db.Set<DbEntity>().Where(where).ToList();
                else
                    dbEntities = db.Set<DbEntity>()
                                            .ToList();
                if (dbEntities != null)
                {
                    //we need to save each one separatelly, to avoid braking the sync between the databases.Its a performance hit but necessary.
                    dbEntities.ForEach(p =>
                    {
                        editValues(p);
                        db.SaveChanges();
                        OnAfterSave(p, db);
                    });
                }
            }
            return dbEntities
                             .Select(p => p.Id)
                             .ToList();
        }
        protected async Task<List<int>> AddMany(List<DbEntity> dbEntitiesToAdd)
        {
            if (dbEntitiesToAdd != null)
            {
                using (DbContext db = await GetDbContext())
                {
                    dbEntitiesToAdd.ForEach(p =>
                    {
                        db.Set<DbEntity>().Add(p);
                    });
                    db.SaveChanges();
                }

                return dbEntitiesToAdd
                                 .Select(p => p.Id)
                                 .ToList();
            }
            else
                return null;
        }
        protected virtual async Task<int> Save(DbEntity entity)
        {
            int dbEntityId = 0;
            try
            {
                dbEntityId = await SaveEntity(entity);
            }
            catch (Exception ex)
            {
                //Logger.Log(ex, 4);
            }
            return dbEntityId;
        }
        protected async virtual Task<bool> Delete(int id, Action<DbEntity, DbContext> onBeforeSave = null)
        {
            try
            {
                using (DbContext db = await GetDbContext())
                {
                    DbEntity dbEntity = GetDbSet(db).Where(p => p.Id == id).FirstOrDefault();
                    if (dbEntity != null)
                        Remove(db, dbEntity);
                    onBeforeSave?.Invoke(dbEntity, db);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected async Task<bool> Delete(List<int> ids)
        {
            try
            {
                using (DbContext db = await GetDbContext())
                {
                    var entities = db.Set<DbEntity>().Where(p => ids.FirstOrDefault(x => x == p.Id) != 0);
                    db.Set<DbEntity>().RemoveRange(entities);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Log(ex);
                return false;
            }
        }
        protected async Task<bool> Delete(Expression<Func<DbEntity, bool>> where)
        {
            try
            {
                using (DbContext db = await GetDbContext())
                {
                    var entities = db.Set<DbEntity>().Where(where);
                    db.Set<DbEntity>().RemoveRange(entities);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        protected async Task<bool> DeleteAll()
        {
            try
            {
                using (DbContext db = await GetDbContext())
                {
                    var entities = db.Set<DbEntity>();
                    db.Set<DbEntity>().RemoveRange(entities);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Log(ex);
                return false;
            }
        }
        protected virtual void Remove(DbContext db, DbEntity entity)
        {
            db.Set<DbEntity>().Remove(entity);
        }
        protected virtual IQueryable<DbEntity> LoadRelations(IQueryable<DbEntity> dbSet)
        {
            return dbSet;
        }
        protected async virtual Task<int> SaveEntity(DbEntity updatedEntity)
        {
            DbEntity dbEntity = default(DbEntity);
            using (DbContext db = await GetDbContext())
            {

                if (updatedEntity.Id == 0)
                {
                    dbEntity = AddNew(db);
                    updatedEntity.Id = dbEntity.Id;
                }
                else
                {
                    dbEntity = GetDbSetForSave(db).Where(p => p.Id == updatedEntity.Id).FirstOrDefault();
                }                
                Map(updatedEntity, dbEntity, db);
                if (dbEntity.GetType().IsSubclassOf(typeof(Tenant)))
                    (dbEntity as Tenant).TenantId = TenantResolver.GetCurrentTenantId();
                OnBeforeSave(dbEntity, db);
                db.SaveChanges();
                OnAfterSave(dbEntity, db);
            }
            return dbEntity.Id;
        }
        protected virtual DbEntity AddNew(DbContext db)
        {
            var newEntity = new DbEntity();
            db.Set<DbEntity>().Add(newEntity);
            return newEntity;
        }
        protected virtual void Map(DbEntity fromEntity, DbEntity toEntity,DbContext dbContext)
        {
            PropertyInfo[] props = fromEntity.GetType().GetProperties();
            foreach (PropertyInfo oneProperty in props)
            {
                var tableColAttribute = oneProperty.GetCustomAttribute(typeof(TableColumnAttr));
                if (tableColAttribute != null && ((TableColumnAttr)tableColAttribute).AutoSave)
                    toEntity.GetType().GetProperty(oneProperty.Name).SetValue(toEntity, oneProperty.GetValue(fromEntity));
            }
        }
        protected virtual IEnumerable<DbEntity> GetDbSetForSave(DbContext db)
        {
            return GetDbSet(db);
        }
        protected async Task<DbEntity> _Get(Expression<Func<DbEntity, bool>> where)
        {
            DbEntity entity = default(DbEntity);
            using (DbContext db = await GetDbContext())
            {
                entity = GetDbSet(db).AsQueryable().Where(where).FirstOrDefault();
            }
            //if (JsonParserInfo.ParseJsonProperties)
            //    ParseEntityWithJsonProperties(entity);
            return entity;
        }
        protected virtual void OnBeforeSave(DbEntity dbEntity, DbContext db)
        {
            if (dbEntity is ITrackChanges)
            {
                var trackChangesAttribute = dbEntity.GetType().GetCustomAttribute(typeof(TrackChangesAttr));
                if (trackChangesAttribute != null && !((TrackChangesAttr)trackChangesAttribute).AutoUpdate)
                    return;
                var changeCounter = (dbEntity as ITrackChanges).ChangeCounter;
                var newChangeCounter = changeCounter.HasValue && changeCounter >= 0 ? changeCounter + 1 : 0;
                (dbEntity as ITrackChanges).ChangeCounter = newChangeCounter;
            }
        }
        protected virtual void OnAfterSave(DbEntity dbEntity, DbContext db) { }
        async protected virtual Task<DbContext> GetDbContext()
        {
            return DbContextFactory.CreateDbContext();
        }
        protected int ExecuteSqlCommand(string command, int? timeout = null)
        {
            throw new NotImplementedException();
        }
        private IEnumerable<DbEntity> GetDbSet(DbContext db)
        {
            if (RelationsEagerLoading)
                return LoadRelations(db.Set<DbEntity>());
            else
                return db.Set<DbEntity>();
        }
    }
}
