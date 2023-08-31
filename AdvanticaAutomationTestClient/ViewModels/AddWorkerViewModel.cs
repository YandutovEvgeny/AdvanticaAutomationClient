using AdvanticaAutomationTestClient.Commands;
using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using Google.Protobuf.WellKnownTypes;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
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

        public System.Action CloseAction { get; set; }
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
                MessageBox.Show("Заполните обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var addedWorker = await _addWorkerService.AddWorkerAsync(new Utis.Minex.WrokerIntegration.WorkerAction
            {
                Worker = new Utis.Minex.WrokerIntegration.WorkerMessage
                {
                    FirstName = Worker.FirstName,
                    LastName = Worker.LastName,
                    MiddleName = Worker.MiddleName ?? string.Empty,
                    Birthday = Timestamp.FromDateTime(Worker.Birthday),
                    Sex = sex,
                    HaveChildren = HaveChildren ? true : false,
                }
            });

            if (addedWorker is WorkerMessage)
            {
                MessageBox.Show($"Рабочий {addedWorker.MiddleName} {addedWorker.FirstName} {addedWorker.LastName} успешно создан",
                    "Рабочий добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction();
                return;
            }

            MessageBox.Show("Упс, что-то пошло не так...", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        });
        #endregion

        private bool ValidateWorkerProps()
        {
            //в бд храним long 
            //при получении лонг парсим в строку, затем в DateTime и наоборот
            if (Worker.FirstName == null || Worker.LastName == null)
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
