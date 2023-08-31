using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Services
{
    public class MainWindowService : IMainWindowService
    {
        private readonly IGrpcService _service;
        public MainWindowService()
        {
            _service = new GrpcService();
        }

        public async Task<List<WorkerServiceModel>> GetWorkersAsync()
        {
            return await _service.GetWorkers();
        }

        public async Task<WorkerMessage> DeleteWorkerAsync(WorkerAction workerAction)
        {
            return await _service.RemoveWorker(workerAction);
        }
    }
}
