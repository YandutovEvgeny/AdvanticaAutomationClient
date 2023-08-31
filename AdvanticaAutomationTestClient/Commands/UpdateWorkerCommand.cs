using System;
using System.Windows.Input;

namespace AdvanticaAutomationTestClient.Commands
{
    public class UpdateWorkerCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _updateWorkerCommand;

        public UpdateWorkerCommand(Action updateWorkerCommand)
        {
            this._updateWorkerCommand = updateWorkerCommand;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _updateWorkerCommand?.Invoke();
        }
    }
}
