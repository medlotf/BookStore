using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreApp.Models.Repositories
{
    public interface IBooksStoreRepository<TEntity>
    {
        IList<TEntity> List();

        TEntity Find(int id);

        void Add(TEntity entity);

        void Update(int id,TEntity newEntity);

        void Delete(int id);
    }
}
