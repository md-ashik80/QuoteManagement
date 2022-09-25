using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ModernQuote.Api.Data;
using System.Linq;

namespace ModernQuote.Api.Test.Data
{
    public class QuoteRequestRepositoryTests
    {
        private QuotesDbContext quotesDbContext;
        private QuoteRequestBuilder quoteReqestBuilder;
        private IQuoteRequestRepository repository;

        [SetUp]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<QuotesDbContext>()
            .UseInMemoryDatabase(databaseName: "QuotesDatabase")
            .Options;

            quotesDbContext = new QuotesDbContext(options);

            quoteReqestBuilder = new QuoteRequestBuilder();

            quotesDbContext.QuoteRequests.Add(new QuoteRequestBuilder().WithSilverPlanQuote().Build());
            quotesDbContext.QuoteRequests.Add(new QuoteRequestBuilder().WithGoldPlanQuote().Build());
            quotesDbContext.QuoteRequests.Add(new QuoteRequestBuilder().WithPlatinumPlanQuote().Build());

            quotesDbContext.SaveChanges();

            repository = new QuoteRequestRepository(quotesDbContext);
        }

        [TearDown]
        public void Cleanup()
        {
            quotesDbContext.Database.EnsureDeleted();
            quotesDbContext.Dispose();
            quotesDbContext = null;
            repository = null;
        }

        [Test]
        public void ListGetAllTheQuoteRequest()
        {
            var requests = repository.List();

            Assert.AreEqual(3, requests.Count);
        }

        [Test]
        public void GetWillRetrieveRequestById()
        {
            var request = repository.Get(1);

            Assert.NotNull(request);
            Assert.AreEqual(1, request.Id);
        }

        [Test]
        public void AddWillAddNewRequest()
        {
            var request = quoteReqestBuilder.WithSilverPlanQuote().Build();

            repository.Add(request);
            quotesDbContext.SaveChanges();

            Assert.AreEqual(4, quotesDbContext.QuoteRequests.Count());
        }

        [Test]
        public void EditWillEditTheGiveRequest()
        {
            var originalRequest = repository.Get(1);
            var modifiedRequest = quoteReqestBuilder.Request();

            originalRequest.Name = modifiedRequest.Name;
            originalRequest.DateOfBirth = modifiedRequest.DateOfBirth;

            repository.Edit(originalRequest);
            quotesDbContext.SaveChanges();

            var changedRequest = repository.Get(originalRequest.Id);

            Assert.AreEqual(modifiedRequest.Name, changedRequest.Name);
            Assert.AreEqual(modifiedRequest.DateOfBirth.Value.Date, changedRequest.DateOfBirth.Value.Date);
        }

        [Test]
        public void DeleteWillRequestRequestById()
        {
            repository.Delete(1);
            repository.SaveChanges();

            var request = repository.Get(1);

            Assert.Null(request);
        }
    }
}
