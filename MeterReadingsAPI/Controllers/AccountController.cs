using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MeterReading.Logic;
using Swashbuckle.Swagger.Annotations;

namespace MeterReadingsAPI.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountFacade _accountFacade;

        public AccountController(IAccountFacade accountFacade)
        {
            _accountFacade = accountFacade ?? throw new ArgumentNullException(nameof(accountFacade));
        }

        // GET: api/Account
        [SwaggerResponse(HttpStatusCode.OK, "Collection of accounts", typeof(IEnumerable<MeterReadings.Schema.Account>))]
        public async Task<IEnumerable<MeterReadings.Schema.Account>> Get()
        {
            return await _accountFacade.GetAccounts();
        }

        // GET: api/Account/5
        [SwaggerResponse(HttpStatusCode.OK, "Account", typeof(MeterReadings.Schema.Account))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> Get(int id)
        {
            var account = await _accountFacade.GetAccount(id);

            if (account != null)
            {
                return new NegotiatedContentResult<MeterReadings.Schema.Account>(HttpStatusCode.OK, account, this);
            }

            return NotFound();
        }

        // POST: api/Account
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IHttpActionResult> Post(MeterReadings.Schema.Account account)
        {
            var accountId = await _accountFacade.AddAccount(account);

            if (accountId.HasValue)
            {
                return new NegotiatedContentResult<int>(HttpStatusCode.Created,
                    accountId.Value, this);
            }

            return BadRequest();
        }

        // DELETE: api/Account/5
        [SwaggerResponse(HttpStatusCode.NoContent)]
        public async Task<HttpStatusCode> Delete(int id)
        {
            await _accountFacade.DeleteAccount(id);

            return HttpStatusCode.NoContent;
        }
    }
}
