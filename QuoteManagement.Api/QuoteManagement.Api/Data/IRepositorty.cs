using System.Collections.Generic;

namespace QuoteManagement.Api.Data
{
    public interface IRepositorty<TEntity> where TEntity : class
    {
        IList<TEntity> List();

        TEntity Get(int id);

        void Add(TEntity entity);

        void Edit(TEntity entity);

        void Delete(int id);

        void SaveChanges();
    }
}
