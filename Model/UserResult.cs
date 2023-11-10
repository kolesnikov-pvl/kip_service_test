using System;
using Newtonsoft.Json;

namespace kip_service_test
{
    public class Response
    {
        [JsonProperty("query")]
        public Guid QueryGuid { get; set; }

        [JsonProperty("percent")]
        public int PercentCompleted { get; set; }

        [JsonProperty("result")]
        public Result Res { get; set; }

        public class Result
        {
            [JsonProperty("user_id")]
            public string UserId { get; set; }

            [JsonProperty("count_sign_in")]
            public int CountSignIn { get; set; }
        }

    }
}