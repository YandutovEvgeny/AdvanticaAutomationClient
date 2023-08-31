using AdvanticaAutomationTestClient.Commands;
using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using AdvanticaAutomationTestClient.Views;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
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

            GetWorkers();
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
        public AddWorkerCommand AddWorkerCommand => new AddWorkerCommand(() =>
        {
            OpenAddWorkerWindow();
        });
        public UpdateWorkerCommand UpdateWorkerCommand => new UpdateWorkerCommand(() =>
        {
            if (SelectedWorker == null)
            {
                MessageBox.Show("Выберите рабочего, которого хотите изменить!", "Рабочий не выбран", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OpenUpdateWorkerWindow();
        });
        public DeleteWorkerCommand DeleteWorkerCommand => new DeleteWorkerCommand(async () => 
        {
            if (SelectedWorker == null)
            {
                MessageBox.Show("Выберите рабочего, которого хотите удалить!", "Рабочий не выбран",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить рабочего?", "Удаление рабочего",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                var deletedWorker = await _service.DeleteWorkerAsync(new Utis.Minex.WrokerIntegration.WorkerAction
                {
                    Worker = new Utis.Minex.WrokerIntegration.WorkerMessage
                    {
                        Id = SelectedWorker.Id,
                        FirstName = SelectedWorker.FirstName, 
                        LastName = SelectedWorker.LastName,
                        MiddleName = SelectedWorker.MiddleName ?? string.Empty,
                        Birthday = Timestamp.FromDateTime(SelectedWorker.Birthday),
                        Sex = SelectedWorker.Sex,
                        HaveChildren = SelectedWorker.HaveChildren
                    }
                });

                if(deletedWorker is WorkerMessage)
                {
                    MessageBox.Show("Рабочий успешно удалён!", "Рабочий удалён", MessageBoxButton.OK, MessageBoxImage.Information );
                    GetWorkers();
                }
            }

        });
        #endregion

        public async void GetWorkers()
        {
            var data = await _service.GetWorkersAsync();
            
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

        private DateTime LongToDateTimeConverter(long value)
        {
            try
            {
                var date = value.ToString(); //15122000
                int day, month, year = 0;

                if (value.ToString().Length == 8)
                {
                    day = Convert.ToInt32(date.Substring(0, date.Length - 6)); //15
                    month = Convert.ToInt32(date.Substring(2, date.Length - 6)); //12
                    year = Convert.ToInt32(date.Substring(4)); //2000

                    return new DateTime(year, month, day);
                }

                //date = "0" + date;
                day = Convert.ToInt32(date.Substring(0, date.Length - 6));
                month = Convert.ToInt32(date.Substring(1, date.Length - 5));
                year = Convert.ToInt32(date.Substring(3));

                return new DateTime(year, month, day);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
        private long DateTimeToLongConverter(DateTime value)
        {
            string result = value.ToShortDateString();  //01.12.1234
           // result = result.Replace('.', /*??*/);

            return Convert.ToInt64(result);
        }

        private void OpenAddWorkerWindow()
        {
            var addWindow = new AddWorkerView();
            addWindow.ShowDialog();
            GetWorkers();
        }

        private void OpenUpdateWorkerWindow()
        {
            UpdateWorkerViewModel.Id = SelectedWorker.Id;
            var updateWindow = new UpdateWorkerView();
            updateWindow.ShowDialog();
            GetWorkers();
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
