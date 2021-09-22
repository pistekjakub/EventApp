using EventApp.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.Data.Repositories
{
    public interface IRegistrationRepository
    {
        public Task<List<RegistrationDto>> GetRegistrations(string eventName);

        public Task<bool> CheckUniqueRegistration(RegistrationDto registration, string eventName);

        public Task<RegistrationDto> InsertRegistration(RegistrationDto registration, string eventName);
    }
}
