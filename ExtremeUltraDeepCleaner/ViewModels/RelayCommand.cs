using System.Windows.Input;

namespace ExtremeUltraDeepCleaner.ViewModels
{
    /// <summary>
    /// Generic ICommand implementation for MVVM command binding
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Event raised when CanExecute status changes
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a new RelayCommand
        /// </summary>
        /// <param name="execute">Action to execute when command is invoked</param>
        /// <param name="canExecute">Optional function to determine if command can execute</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates a new RelayCommand without parameters
        /// </summary>
        /// <param name="execute">Action to execute when command is invoked</param>
        /// <param name="canExecute">Optional function to determine if command can execute</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(_ => execute(), canExecute == null ? null : _ => canExecute())
        {
        }

        /// <summary>
        /// Determines whether the command can execute
        /// </summary>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Raises CanExecuteChanged to refresh command bindings
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
