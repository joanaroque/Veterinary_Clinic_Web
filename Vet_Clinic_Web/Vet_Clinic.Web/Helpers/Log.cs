using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Helpers
{
    public class Log : ILog
    {
        public void Append(string message)
        {
            Console.WriteLine(DateTime.Now+"|"+message);
        }
    }
}
