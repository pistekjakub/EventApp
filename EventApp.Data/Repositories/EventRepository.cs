using EventApp.Data.Dtos;
using EventApp.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IDbContext _databaseContext;

        private readonly ISqlExecutor _sqlExecutor;

        private static class Procedure
        {
            public const string GetEvents = "sp_GetEvents";
            public const string InsertEvent = "sp_InsertEvent";
            public const string CheckUniqueEvent = "sp_CheckUniqueEvent";
        }

        public EventRepository(IDbContext databaseContext, ISqlExecutor sqlExecutor)
        {
            _databaseContext = databaseContext;
            _sqlExecutor = sqlExecutor;
        }

        public async Task<List<EventDto>> GetEvents()
        {
            var eventsList = new List<EventDto>();
            IEnumerable<EventDto> rows;

            using (var connection = await _databaseContext.GetConnection())
            {
                rows = await
                    _sqlExecutor.ExecuteQueryStoredProcedureAsync<EventDto>(connection,
                        Procedure.GetEvents);
            }

            if (rows != null)
            {
                var events = rows as EventDto[] ?? rows.ToArray();

                eventsList.AddRange(events);
            }

            return eventsList;
        }

        public async Task<bool> CheckUniqueEvent(string name)
        {
            name = Helpers.TrimSafe(name);

            var parameters = new
            {
                name,
            };
            int result = (int)decimal.MinusOne;

            using (var connection = await _databaseContext.GetConnection())
            {
                result = await
                    _sqlExecutor.ExecuteScalarStoredProcedureAsync<int>(connection,
                        Procedure.CheckUniqueEvent, parameters);
            }

            return result == decimal.Zero;
        }

        public async Task<EventDto> InsertEvent(EventDto eventDto)
        {
            TrimEvent(eventDto);

            var parameters = new
            {
                name = eventDto.Name,
                description = eventDto.Description,
                location = eventDto.Location,
                startTime = eventDto.StartTime,
                endTime = eventDto.EndTime
            };

            using (var connection = await _databaseContext.GetConnection())
            {
                var newId = await _sqlExecutor.ExecuteScalarStoredProcedureAsync<long>(connection,
                    Procedure.InsertEvent,
                    parameters);
                eventDto.Id = newId;
            }

            return eventDto;
        }

        private void TrimEvent(EventDto eventDto)
        {
            eventDto.Name = Helpers.TrimSafe(eventDto.Name);
            eventDto.Description = Helpers.TrimSafe(eventDto.Description);
            eventDto.Location = Helpers.TrimSafe(eventDto.Location);
        }
    }
}
