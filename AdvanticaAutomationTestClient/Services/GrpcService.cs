using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using Grpc.Core;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Services
{
    public class GrpcService : IGrpcService
    {
        readonly WorkerIntegration.WorkerIntegrationClient _client;

        public GrpcService()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7157");
            _client = new WorkerIntegration.WorkerIntegrationClient(channel);
        }

        public async Task<WorkerMessage> AddWorker(WorkerAction workerAction)
        {
            var addedWorker = await _client.CreateWorkerAsync(workerAction);

            return await Task.FromResult(addedWorker);
        }

        public async Task<WorkerMessage> FindWorkerById(long id)
        {
            var findedWorker = await _client.FindWorkerByIdAsync(new WorkerId { Id = id });

            return await Task.FromResult(findedWorker);
        }

        public async Task<List<WorkerServiceModel>> GetWorkers()
        {
            var serverData = _client.GetWorkerStream(new EmptyMessage());
            var responseStream = serverData.ResponseStream;
            var workers = new List<WorkerServiceModel>();

            try
            {
                await foreach (var response in responseStream.ReadAllAsync())
                {
                    if (response != null)
                    {
                        workers.Add(new WorkerServiceModel
                        {
                            Id = response.Worker.Id,
                            FirstName = response.Worker.FirstName,
                            LastName = response.Worker.LastName,
                            MiddleName = response.Worker.MiddleName,
                            Birthday = response.Worker.Birthday,
                            HaveChildren = response.Worker.HaveChildren,
                            Sex = response.Worker.Sex
                        });
                    }
                }
            }
            catch (System.Exception)
            {

            }

            return await Task.FromResult(workers);
        }

        public async Task<WorkerMessage> RemoveWorker(WorkerAction workerAction)
        {
            var deletedWorker = await _client.DeleteWorkerAsync(workerAction);

            return await Task.FromResult(deletedWorker);
        }
    }
}
