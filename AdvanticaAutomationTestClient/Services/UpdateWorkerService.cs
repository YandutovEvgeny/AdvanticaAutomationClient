using AdvanticaAutomationTestClient.Interfaces;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Services
{
    public class UpdateWorkerService : IUpdateWorkerService
    {
        private readonly IGrpcService _service;

        public UpdateWorkerService()
        {
            _service = new GrpcService();
        }

        public async Task<WorkerMessage> FindWorkerByIdAsync(long id)
        {
            return await _service.FindWorkerById(id);
        }
    }
}
