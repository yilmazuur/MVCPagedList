using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCPagedList.App.Data.Interfaces
{
    public interface IGenericRepository<T> :IDisposable where T : class
    {
        IEnumerable<T> SelectAll();
        IEnumerable<T> SelectSome(int page, int count);
        T SelectByID(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save(string cacheKey);
    }
}
