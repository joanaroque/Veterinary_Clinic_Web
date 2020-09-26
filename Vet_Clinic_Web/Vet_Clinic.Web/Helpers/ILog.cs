using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet_Clinic.Web.Helpers
{
    public interface ILog
    {
         void Append(string message);
    }
}
