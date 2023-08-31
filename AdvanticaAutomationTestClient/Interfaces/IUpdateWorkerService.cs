using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Interfaces
{
    interface IUpdateWorkerService
    {
        Task<WorkerMessage> FindWorkerByIdAsync(long id);
    }
}
