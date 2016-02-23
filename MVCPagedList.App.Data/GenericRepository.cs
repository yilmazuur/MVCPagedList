using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCPagedList.App.Data.DataModel;
using MVCPagedList.App.Data.Helpers;
using MVCPagedList.App.Data.Interfaces;

namespace MVCPagedList.App.Data
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class //, new()
    {
        private readonly MVCPagedListEntities dbContext;
        private DbSet<T> table;

        public GenericRepository()
        {
            try
            {
                this.dbContext = new MVCPagedListEntities();
                table = dbContext.Set<T>();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
            }
        }
        public GenericRepository(MVCPagedListEntities db)
        {
            try
            {
                this.dbContext = db;
                table = db.Set<T>();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
            }
        }
        public IEnumerable<T> SelectAll()
        {
            try
            {
                return table.ToList();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
                return null;
            }
        }

        public IEnumerable<T> SelectSome(int page, int count)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
                return null;
            }
        }

        public T SelectByID(object id)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
                return null;
            }
        }
        public void Insert(T obj)
        {
            try
            {
                table.Add(obj);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
            }
        }
        public void Update(T obj)
        {
            try
            {
                table.Attach(obj);
                dbContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
            }
        }
        public void Delete(object id)
        {
            try
            {
                T existing = table.Find(id);
                table.Remove(existing);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogToDisk(ex.Message);
            }
        }
        public void Save(string cacheKey)
        {
            dbContext.SaveChanges();
            IEnumerable<T> updatedObjects = this.SelectAll();
            CacheHelper.UpdateCache(cacheKey, updatedObjects);
        }
        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
