using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public class Con
    {
        public static string C()
        {
            const string Host = "chuc.caseum.ru";
            const int Port = 33333;
            const string User = "st_2_19_24";
            const string Db = "is_2_19_st24_KURS";
            const string Pass = "16465377";
            string connStr = $"server={Host};" +
            $"port={Port};" +
            $"user={User};" +
            $"database={Db};" +
            $"password={Pass};";
            return connStr;
        }
    }
}
