using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    public class HttpJobDispatcher : IDashboardDispatcher
    {
        public async Task Dispatch([NotNull] DashboardContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                if (!"POST".Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    return;
                }

                var op = context.Request.GetQuery("op");
                var jobName = context.Request.GetQuery("jobName");

                if (string.IsNullOrEmpty(op))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
                var result = false;

                //if (_context is AspNetCoreDashboardContext)
                //{
                //    var context = _context.GetHttpContext();
                //    body = context.Request.Body;
                //}

                switch (op)
                {
                    case "startbackgroundjob":
                        result = JobAgent.StartBackgroundJob(new JobItem { JobName = jobName });
                        break;
                    case "stopbackgroundjob":
                        result = JobAgent.StopBackgroundJob(new JobItem { JobName = jobName });
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        return;
                }

            }
            catch (Exception)
            {

                throw;
            }

            throw new NotImplementedException();
        }

       

    }
}
