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
        }

        public EventRepository(IDbContext databaseContext, ISqlExecutor sqlExecutor)
        {
            _databaseContext = databaseContext;
            _sqlExecutor = sqlExecutor;
        }

        public async Task<List<EventDto>> GetEvents()
        {
            var eventsList = new List<EventDto>();

            // TODO add parameters for potential parameter like fromDate etc ...
            //var parameters = new
            //{
            //    userInformation.TenantId,
            //    excludeDisabled,
            //    chNodeCodes = userInformation.CustomerHierarchyNodeCodes.AsStringList()
            //};
            IEnumerable<EventDto> rows;

            using (_databaseContext.StartTransaction())
            {
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

                await _databaseContext.Commit();
            }

            return eventsList;
        }
    }
}
