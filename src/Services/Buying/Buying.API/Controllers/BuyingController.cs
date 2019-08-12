using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Services.Buying.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Buying.API.Infrastructure.Services;
using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate;

namespace Buying.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Buying")]
    public class BuyingController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IBuyerQueries _buyerQueries;
        private readonly IIdentityService _identityService;

        public BuyingController(IMediator mediator, IBuyerQueries orderQueries, IIdentityService identityService)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _buyerQueries = orderQueries ?? throw new ArgumentNullException(nameof(orderQueries));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }
        // GET: api/Buying
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Buying/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Buying
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Buying/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("cardtypes")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.CardType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCardTypes()
        {
            var cardTypes = await _buyerQueries
                .GetCardTypesAsync();

            return Ok(cardTypes);
        }
    }
}
