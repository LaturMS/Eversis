using System;
using System.Windows.Input;

namespace MVVMExample
{
    public class RelayCommand : ICommand
    {
        private Action _action;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        private Action<object> execute;

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public void ExecuteParameters(object parameter)
        {
            execute(parameter);
        }
    }
}
