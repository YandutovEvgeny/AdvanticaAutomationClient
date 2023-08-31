using System;
using System.Windows.Input;

namespace AdvanticaAutomationTestClient.Commands
{
    public class DeleteWorkerCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _deleteWorkerCommand;

        public DeleteWorkerCommand(Action deleteWorkerCommand)
        {
            this._deleteWorkerCommand = deleteWorkerCommand;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _deleteWorkerCommand?.Invoke();
        }
    }
}
