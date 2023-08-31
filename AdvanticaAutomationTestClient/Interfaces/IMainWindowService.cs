using AdvanticaAutomationTestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Interfaces
{
    public interface IMainWindowService
    {
        Task<List<WorkerServiceModel>> GetWorkersAsync();
        Task<WorkerMessage> DeleteWorkerAsync(WorkerAction workerAction);
        
    }
}
