using Microsoft.EntityFrameworkCore;
using QuoteManagement.Api.Models;

namespace QuoteManagement.Api.Data
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions options) : base(options)
        {

        }

        public QuotesDbContext() { }

        public DbSet<QuoteRequest> QuoteRequests { get; set; }
    }
}
