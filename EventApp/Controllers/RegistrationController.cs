
using EventApp.Models.Requests;
using EventApp.Models.Responses;
using EventApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet("{eventName}")]
        public async Task<ActionResult<GetReqistrationsResponse>> Get(string eventName)
        {
            return await _registrationService.GetRegistrations(new GetRegistrationsRequest { EventName = eventName });
        }

        [HttpPost]
        public async Task<ActionResult<InsertRegistrationResponse>> Post([FromBody] InsertRegistrationRequest request) 
        {
            return await _registrationService.InsertRegistration(request);
        }

    }
}
