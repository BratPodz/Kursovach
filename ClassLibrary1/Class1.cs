using System;

namespace ClassLibrary1
{
    public class Con
    {
        public static string C()
        {
            const string Host = "caseum.ru";
            const int Port = 33333;
            const string User = "st_2_24_19";
            const string Db = "st_2_24_19";
            const string Pass = "65770709";
            string connStr = $"server={Host};" +
            $"port={Port};" +
            $"user={User};" +
            $"database={Db};" +
            $"password={Pass};";
            return connStr;
        }
    }
}