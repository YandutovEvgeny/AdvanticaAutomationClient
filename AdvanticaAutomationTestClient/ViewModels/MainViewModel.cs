using AdvanticaAutomationTestClient.Commands;
using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Localization;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using AdvanticaAutomationTestClient.Views;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IMainWindowService _service;
        private ObservableCollection<WorkerModel> _workers;
        private WorkerModel _selectedWorker;

        public MainViewModel()
        {
            _service = new MainWindowService();
            Workers = new ObservableCollection<WorkerModel>();

            var workers = GetWorkers();
        }

        #region Props
        public ObservableCollection<WorkerModel> Workers
        {
            get => _workers;
            set { _workers = value; Notify(nameof(Workers)); }
        }
        public WorkerModel SelectedWorker
        {
            get => _selectedWorker;
            set { _selectedWorker = value; Notify(nameof(SelectedWorker)); }
        }
        #endregion

        #region Commands
        public AddWorkerCommand AddWorkerCommand => new AddWorkerCommand(async () =>
        {
            await OpenAddWorkerWindow();
        });
        public UpdateWorkerCommand UpdateWorkerCommand => new UpdateWorkerCommand(async () =>
        {
            if (SelectedWorker == null)
            {
                MessageBox.Show(Resources.MainSelectUpdateWorker, Resources.MainWorkerNotSelected, 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            await OpenUpdateWorkerWindow();
        });
        public DeleteWorkerCommand DeleteWorkerCommand => new DeleteWorkerCommand(async () => 
        {
            if (SelectedWorker == null)
            {
                MessageBox.Show(Resources.MainSelectDeleteWorker, Resources.MainWorkerNotSelected,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show(Resources.MainAreYouSureToDeleteWorker, Resources.MainDeletingWorker,
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                var deletedWorker = await _service.DeleteWorkerAsync(new WorkerAction
                {
                    Worker = new WorkerMessage
                    {
                        Id = SelectedWorker.Id,
                        FirstName = SelectedWorker.FirstName, 
                        LastName = SelectedWorker.LastName,
                        MiddleName = SelectedWorker.MiddleName ?? string.Empty,
                        Birthday = Timestamp.FromDateTime(DateTime.SpecifyKind(SelectedWorker.Birthday, DateTimeKind.Utc)),
                        Sex = SelectedWorker.Sex,
                        HaveChildren = SelectedWorker.HaveChildren
                    }
                });

                if(deletedWorker is WorkerMessage)
                {
                    await GetWorkers();
                    MessageBox.Show(Resources.MainWorkerSuccessfullyRemoved, Resources.MainWorkerRemoved, 
                        MessageBoxButton.OK, MessageBoxImage.Information );
                }
            }

        });
        #endregion

        /// <summary>
        /// Метод, позволяющий получить сотрудников с сервера.
        /// </summary>
        private async Task GetWorkers()
        {
            var data = await _service.GetWorkersAsync();

            if (Workers.Count != 0)
            {
                Workers.Clear();
            }

            foreach (var worker in data)
            {
                Workers.Add(new WorkerModel
                {
                    Id = worker.Id,
                    FirstName = worker.FirstName,
                    LastName = worker.LastName,
                    MiddleName = worker.MiddleName,
                    Birthday = worker.Birthday,
                    Sex = worker.Sex,
                    HaveChildren = worker.HaveChildren
                });
            }
        }

        private async Task OpenAddWorkerWindow()
        {
            var addWindow = new AddWorkerView();
            addWindow.ShowDialog();
            await GetWorkers();
        }

        private async Task OpenUpdateWorkerWindow()
        {
            UpdateWorkerViewModel.Id = SelectedWorker.Id;
            var updateWindow = new UpdateWorkerView();
            updateWindow.ShowDialog();
            await GetWorkers();
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
