using System.Security.Cryptography;

namespace kip_service_test
{
    public static class GlobalConst
    {
        public static string ProcessingTimeLimitMilliseconds = "ProcessingTimeLimitMilliseconds";
        public static string ConnectionString = "connectionString";

        public static int ToInt(this string str)
        {
            int @int;
            return int.TryParse(str, out @int) ? @int : 0;
        }
    }
}