using EventApp.Data.Dtos;
using EventApp.Data.Repositories;
using EventApp.Models.Requests;
using EventApp.Models.Responses;
using System;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<GetReqistrationsResponse> GetRegistrations(GetRegistrationsRequest request)
        {
            var registrationsDtos = await _registrationRepository.GetRegistrations(request.EventName);
            var result = new GetReqistrationsResponse
            {
                Registrations = EventApp.EntityMapper.EntityMapper.MapRegistrationsDtos(registrationsDtos)
            };
            return result;
        }

        public async Task<InsertRegistrationResponse> InsertRegistration(InsertRegistrationRequest request)
        {
            var registrationDto = new RegistrationDto
            {
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
            };
            bool isUnique = await _registrationRepository.CheckUniqueRegistration(registrationDto, request.EventName);
            if (!isUnique) 
            {
                throw new ArgumentException($"Email {request.Email} or Phone {request.Phone} of event ${request.EventName} is not uniques");
            }

            var insertedDto = await _registrationRepository.InsertRegistration(registrationDto, request.EventName);

            return new InsertRegistrationResponse 
            {
                Inserted = (insertedDto.Id != decimal.MinusOne)
            };
        }
    }
}
