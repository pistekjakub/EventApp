using EventApp.Models.Requests;
using EventApp.Models.Responses;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public interface IRegistrationService
    {
        public Task<GetReqistrationsResponse> GetRegistrations(GetRegistrationsRequest request);

        public Task<InsertRegistrationResponse> InsertRegistration(InsertRegistrationRequest request);
    }
}
