using AdvanticaAutomationTestClient.Interfaces;
using AdvanticaAutomationTestClient.Models;
using AdvanticaAutomationTestClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

            SetSelectedWorker();
        }

        #region Props
        public static long Id { get; set; }

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

        private async Task SetSelectedWorker()
        {
            var data = await _service.FindWorkerByIdAsync(Id);

            SelectedWorker = new WorkerServiceModel
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                MiddleName = data.MiddleName,
                Birthday = data.Birthday,
                HaveChildren = data.HaveChildren,
                Sex = data.Sex
            };

            HaveChildren = SelectedWorker.HaveChildren ? true : false;
            DontHaveChildren = SelectedWorker.HaveChildren ? false : true;
            
            switch(SelectedWorker.Sex) 
            {
                case Sex.Male: IsMan = true; break;
                case Sex.Female: IsWoman = true; break;
                case Sex.DefaultSex:
                    IsMan = false;
                    IsWoman = false; break;
            }
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
