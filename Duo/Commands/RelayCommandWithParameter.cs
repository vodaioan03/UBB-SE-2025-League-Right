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
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
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

            if (parameter == null)
            {
                if (default(T) == null)
                {
                    return canExecute(default);
                }
                return false;
            }

            try
            {
                object converted = Convert.ChangeType(parameter, typeof(T));
                if (converted is T typedConverted)
                {
                    return canExecute(typedConverted);
                }
            }
            catch
            {
                // swallow conversion errors
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is T typedParameter)
            {
                execute(typedParameter);
                return;
            }

            if (parameter == null)
            {
                if (default(T) == null)
                {
                    execute(default);
                    return;
                }

                throw new ArgumentException($"Null parameter not allowed for value type {typeof(T)}", nameof(parameter));
            }

            throw new ArgumentException($"Invalid parameter type. Expected {typeof(T)}.", nameof(parameter));
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
