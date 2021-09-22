
using EventApp.Models.Requests;
using EventApp.Models.Responses;
using EventApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService;
            _logger = logger;
        }

        [HttpGet("{eventName}")]
        public async Task<ActionResult<GetReqistrationsResponse>> Get(string eventName)
        {
            return await _registrationService.GetRegistrations(new GetRegistrationsRequest { EventName = eventName });
        }

        [HttpPost]
        public async Task<ActionResult<InsertRegistrationResponse>> Post([FromBody] InsertRegistrationRequest request) 
        {
            try
            {
                return await _registrationService.InsertRegistration(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new InsertRegistrationResponse
                {
                    Error = ex.Message
                };
            }
        }
    }
}
