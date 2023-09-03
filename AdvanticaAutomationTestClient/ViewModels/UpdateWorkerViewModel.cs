using AdvanticaAutomationTestClient.Commands;
using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using Google.Protobuf.WellKnownTypes;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.ViewModels
{
    public class UpdateWorkerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IUpdateWorkerService _service;
        private WorkerServiceModel _selectedWorker;
        private bool _haveChildren;
        private bool _dontHaveChildren;
        private bool _isMan;
        private bool _isWoman;

        public UpdateWorkerViewModel()
        {
            _service = new UpdateWorkerService();

            Task.Run(() => SetSelectedWorker());
        }

        #region Props
        public static long Id { get; set; }
        public Action CloseAction { get; set; }

        public WorkerServiceModel SelectedWorker
        {
            get => _selectedWorker;
            set { _selectedWorker = value; Notify(nameof(SelectedWorker)); }
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
            get => _isWoman;
            set { _isWoman = value; Notify(nameof(IsWoman)); }
        }

        #endregion

        #region Commands
        public UpdateWorkerCommand UpdateWorkerCommand => new UpdateWorkerCommand(async () => 
        {
            var isValid = ValidateWorkerProps();

            if (!isValid)
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var worker = new WorkerMessage
            {
                Id = SelectedWorker.Id,
                FirstName = SelectedWorker.FirstName,
                MiddleName = SelectedWorker.MiddleName,
                LastName = SelectedWorker.LastName,
                Birthday = Timestamp.FromDateTime(SelectedWorker.Birthday),
                HaveChildren = HaveChildren ? true : false
            };

            if (IsMan) { worker.Sex = Sex.Male; }
            else if(IsWoman) { worker.Sex = Sex.Female; }
            else { worker.Sex = Sex.Default; }

            var updatedWorker = await _service.EditWorkerAsync(new WorkerAction { Worker = worker });

            if(updatedWorker is WorkerMessage)
            {
                MessageBox.Show($"Рабочий {updatedWorker.LastName} {updatedWorker.FirstName} {updatedWorker.MiddleName} успешно изменен",
                    "Рабочий изменен", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction();
                return;
            }

            MessageBox.Show("Упс, что-то пошло не так, обратитесь к администратору", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            CloseAction();
        });
        #endregion

        private async Task SetSelectedWorker()
        {
            var data = await _service.FindWorkerByIdAsync(Id);

            SelectedWorker = new WorkerServiceModel
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                MiddleName = data.MiddleName,
                Birthday = data.Birthday.ToDateTime().Date,
                HaveChildren = data.HaveChildren,
                Sex = data.Sex
            };

            HaveChildren = SelectedWorker.HaveChildren ? true : false;
            DontHaveChildren = SelectedWorker.HaveChildren ? false : true;
            
            switch(SelectedWorker.Sex) 
            {
                case Sex.Male: IsMan = true; break;
                case Sex.Female: IsWoman = true; break;
                case Sex.Default:
                    IsMan = false;
                    IsWoman = false; break;
            }
        }

        private bool ValidateWorkerProps()
        {
            if (string.IsNullOrEmpty(SelectedWorker.FirstName) || string.IsNullOrEmpty(SelectedWorker.LastName))
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
