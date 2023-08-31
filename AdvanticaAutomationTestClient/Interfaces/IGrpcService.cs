using AdvanticaAutomationTestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Interfaces
{
    public interface IGrpcService
    {
        Task<List<WorkerServiceModel>> GetWorkers();
        Task<WorkerMessage> AddWorker(WorkerAction workerAction);
        Task<WorkerMessage> RemoveWorker(WorkerAction workerAction);
        Task<WorkerMessage> FindWorkerById(long id);
    }
}
