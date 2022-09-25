using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ModernQuote.Api.Controllers;
using ModernQuote.Api.Data;
using ModernQuote.Api.Models;
using ModernQuote.Api.Rules;

namespace ModernQuote.Api.Test.Controllers
{
    public class QuotesControllerTests
    {
        [TestCase]
        public void GetQuoteRequestsReturnsAllTheQuote()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quotes = new QuoteRequestBuilder().ManyRequest();

            repository.Setup(r => r.List()).Returns(quotes);

            var controller = new QuotesController(repository.Object, null);
            var results = controller.GetQuoteRequests();

            Assert.AreEqual(results.Count(), quotes.Count);
            Assert.IsTrue(results.All(c => quotes.Any(q => q.Name == c.Name && q.Id == c.Id)), "Expected Quote is not returned");

        }

        [TestCase]
        public void GetQuoteRequestQuoteRequestExistsOkResultWithIdentifiedQuote()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder().Request();

            repository.Setup(r => r.Get(quote.Id)).Returns(quote);

            var controller = new QuotesController(repository.Object, null);
            var result = controller.GetQuoteRequest(quote.Id);

            Assert.IsInstanceOf<OkObjectResult>(result);

            var retrievedQuote = (result as OkObjectResult).Value as QuoteRequest;
            Assert.IsTrue(quote.Name == retrievedQuote.Name && quote.Id == retrievedQuote.Id, "Expected Quote is not returned");
        }

        [TestCase]
        public void GetQuoteRequestQuoteRequestNotExistsNotFoundResultReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            QuoteRequest quote = null;

            repository.Setup(r => r.Get(It.IsAny<int>())).Returns(quote);

            var controller = new QuotesController(repository.Object, null);
            var result = controller.GetQuoteRequest(8);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual((result as NotFoundObjectResult).Value.ToString(), "QuoteId 8 is not found in the system");
        }

        [TestCase]
        public void PostQuoteRequestValidRequestGivenThenQuoteSavedAndContentReturned()
        {
            var calculater = new Mock<IMaturityAmountCalculator>();
            var expectedMaturityAmount = 400000.0m;

            calculater.Setup(c => c.Execute(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<decimal>())).Returns(expectedMaturityAmount);

            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .WithPastQuoteDate()
                .WithMaturityAmount(1000.0m)
                .Build();

            var controller = new QuotesController(repository.Object, calculater.Object);

            var result = controller.PostQuoteRequest(quote, null);
                        
            repository.Verify(r => r.Add(quote), Times.Once, "Quote is not added");
            repository.Verify(r => r.SaveChanges(), Times.Once, "Quote is not saved");

            Assert.IsInstanceOf<CreatedAtActionResult>(result);

            var retrievedQuote = (result as CreatedAtActionResult).Value as QuoteRequest;
            Assert.IsTrue(quote.Name == retrievedQuote.Name && quote.Id == retrievedQuote.Id, "Expected Quote is not Added");
            Assert.AreEqual(DateTime.Today, quote.QuoteDate.Date, "QuoteDate is not set to Today.");
            Assert.AreEqual(expectedMaturityAmount, quote.MaturityAmount, "Expected MaturityAmount is not set");
        }

        [TestCase]
        public void PostQuoteRequestInvalidRequestGivenThenQuoteNotSavedAndBadRequestReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .WithIneligibleSilverPlanRetirementAge()
                .Build();

            var controller = new QuotesController(repository.Object, null);

            controller.ModelState.AddModelError("RetirementAge", "RetirementAge is not valid.");
            var result = controller.PostQuoteRequest(quote, null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            repository.Verify(r => r.Add(quote), Times.Never, "Quote is not expected to add");
            repository.Verify(r => r.SaveChanges(), Times.Never, "Quote is not expected to save");

            var modelState = (result as BadRequestObjectResult).Value as SerializableError;

            Assert.IsTrue(modelState.ContainsKey("RetirementAge"), "Expected Validation Error is not returned");
        }

        [TestCase]
        public void PutQuoteRequestInvalidRequestGivenThenQuoteNotSavedAndBadRequestReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .WithIneligibleSilverPlanRetirementAge()
                .Build();

            var controller = new QuotesController(repository.Object, null);

            controller.ModelState.AddModelError("RetirementAge", "RetirementAge is not valid.");
            var result = controller.PutQuoteRequest(quote.Id, quote);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            repository.Verify(r => r.Add(quote), Times.Never, "Quote is not expected to Edit");
            repository.Verify(r => r.SaveChanges(), Times.Never, "Quote is not expected to save");

            var modelState = (result as BadRequestObjectResult).Value as SerializableError;

            Assert.IsTrue(modelState.ContainsKey("RetirementAge"), "Expected Validation Error is not returned");
        }

        [TestCase]
        public void PutQuoteRequestGivenWithMismatchingIdThenQuoteNotSavedAndBadRequestReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .Build();


            var controller = new QuotesController(repository.Object, null);

            var result = controller.PutQuoteRequest(8, quote);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            Assert.AreEqual((result as BadRequestObjectResult).Value.ToString(), "QuoteId is mismatch");

            repository.Verify(r => r.Add(quote), Times.Never, "Quote is not expected to Edit");
            repository.Verify(r => r.SaveChanges(), Times.Never, "Quote is not expected to save");
        }

        [TestCase]
        public void PutQuoteRequestGivenWithNonExistingRequestThenQuoteNotSavedAndNotFoundReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .Build();

            QuoteRequest existingQuote = null;

            repository.Setup(r => r.Get(quote.Id)).Returns(existingQuote);

            var controller = new QuotesController(repository.Object, null);

            var result = controller.PutQuoteRequest(quote.Id, quote);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual((result as NotFoundObjectResult).Value.ToString(), $"QuoteId {quote.Id} is not found in the system");

            repository.Verify(r => r.Add(quote), Times.Never, "Quote is not expected to Edit");
            repository.Verify(r => r.SaveChanges(), Times.Never, "Quote is not expected to save");
        }

        [TestCase]
        public void PutQuoteRequestGivenWithValidEditedRequestThenQuoteSavedAndOkReturned()
        {
            var calculater = new Mock<IMaturityAmountCalculator>();
            var expectedMaturityAmount = 400000.0m;

            calculater.Setup(c => c.Execute(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<decimal>())).Returns(expectedMaturityAmount);

            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .WithPastQuoteDate()
                .WithMaturityAmount(20000.0m)
                .Build();

            repository.Setup(r => r.Get(quote.Id)).Returns(quote);

            var controller = new QuotesController(repository.Object, calculater.Object);

            var result = controller.PutQuoteRequest(quote.Id, quote);

            Assert.IsInstanceOf<OkResult>(result);

            repository.Verify(r => r.Edit(quote), Times.Once, "Quote is expected to Edit");
            repository.Verify(r => r.SaveChanges(), Times.Once, "Quote is expected to save");

            Assert.AreEqual(DateTime.Today, quote.QuoteDate.Date, "QuoteDate is not set to Today.");
            Assert.AreEqual(expectedMaturityAmount, quote.MaturityAmount, "Expected MaturityAmount is not set");
        }

        [TestCase]
        public void DeleteQuoteRequestGivenWithNonExistingRequestThenQuoteNotDeletedAndNotFoundReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .Build();

            QuoteRequest existingQuote = null;

            repository.Setup(r => r.Get(quote.Id)).Returns(existingQuote);

            var controller = new QuotesController(repository.Object, null);

            var result = controller.DeleteQuoteRequest(quote.Id);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual((result as NotFoundObjectResult).Value.ToString(), $"QuoteId {quote.Id} is not found in the system");

            repository.Verify(r => r.Delete(quote.Id), Times.Never, "Quote is not expected to Delete");
            repository.Verify(r => r.SaveChanges(), Times.Never, "Quote is not expected to save");
        }

        [TestCase]
        public void DeleteQuoteRequestGivenWithExistingRequestThenQuoteDeletedAndOkReturned()
        {
            var repository = new Mock<IQuoteRequestRepository>();
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .Build();

            repository.Setup(r => r.Get(quote.Id)).Returns(quote);

            var controller = new QuotesController(repository.Object, null);

            var result = controller.DeleteQuoteRequest(quote.Id);

            Assert.IsInstanceOf<OkResult>(result);

            repository.Verify(r => r.Delete(quote.Id), Times.Once, "Quote is expected to Delete");
            repository.Verify(r => r.SaveChanges(), Times.Once, "Quote is expected to save");
        }
    }
}
