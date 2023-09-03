using System;
using System.Windows.Input;

namespace AdvanticaAutomationTestClient.Commands
{
    public class AddWorkerCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _addWorkerCommand;

        public AddWorkerCommand(Action addWorkerCommand)
        {
            this._addWorkerCommand = addWorkerCommand;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _addWorkerCommand?.Invoke();
        }
    }
}
