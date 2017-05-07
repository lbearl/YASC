using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YASC.Services
{
    public interface IAlertingService
    {
        void AddJob(string url, string emailAddress);
    }
}
