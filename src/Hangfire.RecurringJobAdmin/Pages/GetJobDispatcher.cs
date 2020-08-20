using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class GetJobDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public GetJobDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }
        public async Task Dispatch([NotNull] DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }

            var recurringJobs = _connection.GetRecurringJobs();

            var periodicJobs = recurringJobs
                .Select(x => new PeriodicJob
                {
                    Id = x.Id,
                    Cron = x.Cron,
                    CreatedAt = x.CreatedAt,
                    Error = x.Error,
                    LastExecution = x.LastExecution,
                    Method = x.Job.Method.Name,
                    Class = x.Job.Method.ReflectedType.FullName,
                    Queue = x.Queue,
                    LastJobId = x.LastJobId,
                    LastJobState = x.LastJobState,
                    NextExecution = x.NextExecution,
                    Removed = x.Removed,
                    TimeZoneId = x.TimeZoneId
                })
                .ToList();

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJobs));
        }
    }
}
