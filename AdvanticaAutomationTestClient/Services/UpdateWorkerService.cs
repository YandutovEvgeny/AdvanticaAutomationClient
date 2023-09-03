using AdvanticaAutomationTestClient.Interfaces;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Services
{
    public class UpdateWorkerService : IUpdateWorkerService
    {
        private readonly IGrpcService _grpcService;

        public UpdateWorkerService()
        {
            _grpcService = new GrpcService();
        }

        public async Task<WorkerMessage> FindWorkerByIdAsync(long id)
        {
            return await _grpcService.FindWorkerById(id);
        }

        public async Task<WorkerMessage> EditWorkerAsync(WorkerAction workerAction)
        {
            return await _grpcService.EditWorker(workerAction);
        }
    }
}
