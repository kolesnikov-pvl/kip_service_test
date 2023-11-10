using System;

namespace kip_service_test
{
    public class UserStatisticsRequest
    {
        public string UserId { get; set; }
        public DateTime StartDate { get; set; } // not used within the task
        public DateTime EndDate { get; set; }  // not used within the task
    }
}