using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Interfaces
{
    interface IUpdateWorkerService
    {
        Task<WorkerMessage> FindWorkerByIdAsync(long id);
        Task<WorkerMessage> EditWorkerAsync(WorkerAction workerAction);
    }
}
