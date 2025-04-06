using System;
using System.Windows.Input;

namespace Duo.Commands
{
    public class RelayCommandWithParameter<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;

        public RelayCommandWithParameter(Action<T> execute, Predicate<T> canExecute = null)
        {
            execute = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            if (parameter is T typedParameter)
            {
                return canExecute(typedParameter);
            }

            if (parameter == null && default(T) == null)
            {
                return canExecute(default);
            }

            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is T typedParameter)
            {
                execute(typedParameter);
            }
            else if (parameter == null && default(T) == null)
            {
                execute(default);
            }
            else
            {
                throw new ArgumentException($"Invalid parameter type. Expected {typeof(T)}.", nameof(parameter));
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
