using Microsoft.AspNetCore.Mvc;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core.Block.Logging;
using Incoding.Core.Block.Logging.Core;
using NHibernate;

namespace kip_service_test.Controllers
{
    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ISessionFactory sessinoFactory { get; set; }

        public ReportController(ISessionFactory sf)
        {
            sessinoFactory = sf;
        }

        private static readonly int delayTime =  ConfigurationManager.AppSettings[GlobalConst.ProcessingTimeLimitMilliseconds].ToInt();
        private static int counter = 0;

        [HttpPost("user_statistics")]
        public ActionResult<Guid> GetUserStatistics([FromBody] UserStatisticsRequest request)
        {
            var requestId = Guid.NewGuid();
            var reportRequest = new ReportRequest
                                {
                                    QueryGuid = requestId,
                                    UserId = request.UserId,
                                    StartTime = DateTime.UtcNow,
                                    EndTime = DateTime.UtcNow,
                                };

            using (var session = sessinoFactory.OpenSession())
                session.Save(reportRequest);

            Task.Factory.StartNew(() => ProcessUserStatistics(requestId));

            return Ok(requestId);
        }

        [HttpGet("info")]
        public ActionResult<Response> GetReportInfo(Guid query)
        {
            try
            {
                ReportRequest rr;
                using (var session = sessinoFactory.OpenSession())
                    rr = session.Query<ReportRequest>().SingleOrDefault(new ReportRequest.Where.ByQueryGuid(query).IsSatisfiedBy()) ?? new ReportRequest();

                var response = new Response()
                {
                    QueryGuid = query,
                    PercentCompleted = rr.IsCompleted ? 100 : CalculatePercentCompleted(rr.StartTime)
                };

                if (rr.IsCompleted)
                    response.Res = new Response.Result
                    {
                        CountSignIn = counter,
                        UserId = rr.UserId
                    };
                
                return Ok(response);
            }
            catch (Exception e)
            {
                LoggingFactory.Instance.LogException(LogType.Debug, e);
            }
            finally
            {
                counter++;
            }

            return NotFound();
        }

        private async Task ProcessUserStatistics(Guid reqId)
        {
            await Task.Delay(delayTime);

            using (var session = sessinoFactory.OpenSession())
            {
                var rr = session.Query<ReportRequest>().SingleOrDefault(new ReportRequest.Where.ByQueryGuid(reqId).IsSatisfiedBy()) ?? new ReportRequest();
                rr.IsCompleted = true;
                session.SaveOrUpdate(rr);
            }
        }

        private int CalculatePercentCompleted(DateTime? start)
        {
            var delta = (DateTime.UtcNow - start.GetValueOrDefault(DateTime.UtcNow)).TotalMilliseconds; 
            return (int)(delta / delayTime * 100);
        }
    }
    //datetime string example for post query  "2023-01-03T00:30:00"

    // мы использовали юнит тесты NCrunch для command и query.  
    // так же мы использовали фреймворк свой Incoding Framework,
    // где все запросы в базу были обернуты в Repository.Query(whereSpecifications.., orderSpecs..).select().tolist.. и Repository.Push(..)
}
