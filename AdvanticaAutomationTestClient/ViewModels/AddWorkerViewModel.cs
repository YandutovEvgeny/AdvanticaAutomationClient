using AdvanticaAutomationTestClient.Commands;
using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Localization;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using Google.Protobuf.WellKnownTypes;
using System;
using System.ComponentModel;
using System.Windows;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.ViewModels
{
    public class AddWorkerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IAddWorkerService _addWorkerService;
        private WorkerServiceModel _worker;

        private bool _haveChildren = true;
        private bool _dontHaveChildren;
        private bool _isMan;
        private bool _isWomen;

        public AddWorkerViewModel()
        {
            _addWorkerService = new AddWorkerService();
            Worker = new WorkerServiceModel();
        }

        #region Props

        public Action CloseAction { get; set; }
        public WorkerServiceModel Worker
        {
            get => _worker;
            set { _worker = value; Notify(nameof(Worker)); }
        }

        public bool HaveChildren
        {
            get => _haveChildren;
            set { _haveChildren = value; Notify(nameof(HaveChildren)); }
        }

        public bool DontHaveChildren
        {
            get => _dontHaveChildren;
            set { _dontHaveChildren = value; Notify(nameof(DontHaveChildren)); }
        }

        public bool IsMan
        {
            get => _isMan;
            set { _isMan = value; Notify(nameof(IsMan)); }
        }

        public bool IsWoman
        {
            get => _isWomen;
            set { _isWomen = value; Notify(nameof(IsWoman)); }
        }
        #endregion

        #region Commands
        public AddWorkerCommand AddWorkerCommand => new AddWorkerCommand(async () =>
        {
            var isValid = ValidateWorkerProps();
            var sex = Sex.Default;

            if (IsMan)
            {
                sex = Sex.Male;
            }
            else if (IsWoman)
            {
                sex = Sex.Female;
            }

            if (!isValid)
            {
                MessageBox.Show(Resources.AddWorkerFillRequiredFields, Resources.AddWorkerError, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addedWorker = await _addWorkerService.AddWorkerAsync(new WorkerAction
            {
                Worker = new WorkerMessage
                {
                    FirstName = Worker.FirstName,
                    LastName = Worker.LastName,
                    MiddleName = Worker.MiddleName ?? string.Empty,
                    Birthday = Timestamp.FromDateTime(DateTime.SpecifyKind(Worker.Birthday, DateTimeKind.Utc)),
                    Sex = sex,
                    HaveChildren = HaveChildren ? true : false,
                }
            });

            if (addedWorker is WorkerMessage)
            {
                MessageBox.Show(Resources.AddWorkerWorker + addedWorker.LastName + " " + addedWorker.FirstName + 
                    " " + addedWorker.MiddleName + Resources.AddWorkerSuccessfullyCreated,
                    Resources.AddWorkerWorkerSuccessfullyAdded, MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction();
                return;
            }

            MessageBox.Show(Resources.AddWorkerTakeAdministrator, Resources.AddWorkerError,
                MessageBoxButton.OK, MessageBoxImage.Error);
            CloseAction();
        });
        #endregion


        /// <summary>
        /// Метод, проверяющий обязательные поля.
        /// </summary>
        /// <returns>true, если все обязательные поля заполнены, иначе false.</returns>
        private bool ValidateWorkerProps()
        {
            if (string.IsNullOrEmpty(Worker.FirstName) || string.IsNullOrEmpty(Worker.LastName))
            {
                return false;
            }

            return true;
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
