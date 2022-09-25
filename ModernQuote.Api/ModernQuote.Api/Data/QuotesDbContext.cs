using Microsoft.EntityFrameworkCore;
using ModernQuote.Api.Models;

namespace ModernQuote.Api.Data
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
