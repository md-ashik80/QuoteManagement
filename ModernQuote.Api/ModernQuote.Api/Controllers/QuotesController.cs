using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ModernQuote.Api.Data;
using ModernQuote.Api.Models;
using ModernQuote.Api.Rules;

namespace ModernQuote.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteRequestRepository _quoteRequestRepository;
        private readonly IMaturityAmountCalculator _maturityAmountCalculator;

        public QuotesController(IQuoteRequestRepository quoteRequestRepository, IMaturityAmountCalculator maturityAmountCalculator)
        {
            _quoteRequestRepository = quoteRequestRepository;
            _maturityAmountCalculator = maturityAmountCalculator;
        }

        // GET: api/Quotes
        [HttpGet]
        public IEnumerable<QuoteRequest> GetQuoteRequests()
        {
            return _quoteRequestRepository.List();
        }

        // GET: api/Quotes/5
        [HttpGet("{id}")]
        public IActionResult GetQuoteRequest([FromRoute] int id)
        {
            var quoteRequest = _quoteRequestRepository.Get(id);

            if (quoteRequest == null)
            {
                return NotFound($"QuoteId {id} is not found in the system");
            }

            return Ok(quoteRequest);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult PutQuoteRequest([FromRoute] int id, [FromBody] QuoteRequest quoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quoteRequest.Id)
            {
                return BadRequest("QuoteId is mismatch");
            }

            if (!QuoteRequestExists(id))
            {
                return NotFound($"QuoteId {id} is not found in the system");
            }

            UpdateMaturityAmount(quoteRequest);

            _quoteRequestRepository.Edit(quoteRequest);
            _quoteRequestRepository.SaveChanges();

            return Ok();
        }

        private void UpdateMaturityAmount(QuoteRequest quoteRequest)
        {
            quoteRequest.QuoteDate = DateTime.Now;
            quoteRequest.MaturityAmount = _maturityAmountCalculator.Execute(quoteRequest.PensionPlan, quoteRequest.DateOfBirth.Value,
                                                                            quoteRequest.QuoteDate, quoteRequest.RetirementAge.Value,
                                                                            quoteRequest.InvestmentAmount.Value);
        }

        // POST: api/Quotes
        [HttpPost]
        public IActionResult PostQuoteRequest([FromBody] QuoteRequest quoteRequest, [FromRoute]ApiVersion version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UpdateMaturityAmount(quoteRequest);
            _quoteRequestRepository.Add(quoteRequest);
            _quoteRequestRepository.SaveChanges();

            //return CreatedAtAction("PostQuoteRequest", new { id = quoteRequest.Id, version = version }, quoteRequest);
            return CreatedAtAction("PostQuoteRequest", quoteRequest);
        }

        // DELETE: api/Quotes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteQuoteRequest([FromRoute] int id)
        {
            if (!QuoteRequestExists(id))
            {
                return NotFound($"QuoteId {id} is not found in the system");
            }

            _quoteRequestRepository.Delete(id);
            _quoteRequestRepository.SaveChanges();

            return Ok();
        }

        private bool QuoteRequestExists(int id)
        {
            return _quoteRequestRepository.Get(id) != null;
        }
    }
}