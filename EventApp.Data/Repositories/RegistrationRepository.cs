using EventApp.Data.Dtos;
using EventApp.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.Data.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly IDbContext _databaseContext;

        private readonly ISqlExecutor _sqlExecutor;

        private static class Procedure
        {
            public const string GetRegistrations = "sp_GetRegistrations";
            public const string CheckUniqueRegistration = "sp_CheckUniqueRegistration";
            public const string InsertRegistration = "sp_InsertRegistration";
        }

        public RegistrationRepository(IDbContext databaseContext, ISqlExecutor sqlExecutor)
        {
            _databaseContext = databaseContext;
            _sqlExecutor = sqlExecutor;
        }

        public async Task<List<RegistrationDto>> GetRegistrations(string eventName)
        {
            var registrationsList = new List<RegistrationDto>();

            var parameters = new
            {
                eventName
            };

            IEnumerable<RegistrationDto> rows;

            using (_databaseContext.StartTransaction())
            {
                using (var connection = await _databaseContext.GetConnection())
                {
                    rows = await
                        _sqlExecutor.ExecuteQueryStoredProcedureAsync<RegistrationDto>(connection,
                            Procedure.GetRegistrations, parameters);
                }

                if (rows != null)
                {
                    var registrations = rows as RegistrationDto[] ?? rows.ToArray();

                    registrationsList.AddRange(registrations);
                }

                await _databaseContext.Commit();
            }

            return registrationsList;
        }

        public async Task<bool> CheckUniqueRegistration(RegistrationDto registration, string eventName)
        {
            TrimRegistration(registration);

            var parameters = new
            {
                phone = registration.Phone,
                email = registration.Email,
                eventName,
            };
            int result = -1;

            using (_databaseContext.StartTransaction())
            {
                using (var connection = await _databaseContext.GetConnection())
                {
                    result = await
                        _sqlExecutor.ExecuteScalarStoredProcedureAsync<int>(connection,
                            Procedure.CheckUniqueRegistration, parameters);
                }
            }

            return result == 0;
        }

        public async Task<RegistrationDto> InsertRegistration(RegistrationDto registration, string eventName)
        {
            TrimRegistration(registration);
            var parameters = new
            {
                name = registration.Name,
                phone = registration.Phone,
                email = registration.Email,
                eventName
            };

            using (_databaseContext.StartTransaction())
            {
                using (var connection = await _databaseContext.GetConnection())
                {
                    var newId = await _sqlExecutor.ExecuteScalarStoredProcedureAsync<int>(connection,
                        Procedure.InsertRegistration,
                        parameters);
                    registration.Id = newId;
                }
                await _databaseContext.Commit();
            }

            return registration;
        }

        private string TrimSafe(string? inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                return inputString.Trim();
            }

            return inputString;
        }

        private void TrimRegistration(RegistrationDto registration)
        {
            registration.Name = TrimSafe(registration.Name);
            registration.Email = TrimSafe(registration.Email);
            registration.Phone = TrimSafe(registration.Phone);
        }
    }
}
