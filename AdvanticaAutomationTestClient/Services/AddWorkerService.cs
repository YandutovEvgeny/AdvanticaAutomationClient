using AdvanticaAutomationTestClient.Interfaces;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Services
{
    public class AddWorkerService : IAddWorkerService
    {
        private readonly IGrpcService _grpcService;

        public AddWorkerService()
        {
            _grpcService = new GrpcService();
        }

        public async Task<WorkerMessage> AddWorkerAsync(WorkerAction workerAction)
        {
            return await _grpcService.AddWorker(workerAction);
        }
    }
}
