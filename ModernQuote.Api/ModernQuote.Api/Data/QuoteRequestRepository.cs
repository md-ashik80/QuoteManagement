using System.Collections.Generic;
using System.Linq;
using ModernQuote.Api.Models;

namespace ModernQuote.Api.Data
{
    public class QuoteRequestRepository : IQuoteRequestRepository
    {
        private readonly QuotesDbContext dbContext;

        public QuoteRequestRepository(QuotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<QuoteRequest> List()
        {
            return dbContext.QuoteRequests.ToList();
        }

        public QuoteRequest Get(int id)
        {
            var entity = dbContext.QuoteRequests.Find(id);

            if (entity != null)
            {
                dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            return entity;
        }

        public void Add(QuoteRequest entity)
        {
            dbContext.QuoteRequests.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = Get(id);

            if( entity != null)
            {
                dbContext.QuoteRequests.Remove(entity);
            }
        }

        public void Edit(QuoteRequest entity)
        {
            dbContext.QuoteRequests.Update(entity);
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
