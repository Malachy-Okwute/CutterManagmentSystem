using System.Windows.Input;

namespace CMS
{
    /// <summary>
    /// A relay command implementation 
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The action to run
        /// </summary>
        private readonly Action? _executeCommand;

        /// <summary>
        /// Action with parameter to run
        /// </summary>
        private readonly Action<object>? _executeCommandWithParameter;

        /// <summary>
        /// The logic determining if a command or an action should be allowed to execute
        /// </summary>
        private readonly Predicate<object> _canExecuteCommand;

        /// <summary>
        /// Event that gets fired when can execute changes
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executeCommand">The action to execute</param>
        /// <param name="canExecuteCommand">The condition whether to execute a command or not </param>
        public RelayCommand(Action executeCommand, Predicate<object> canExecuteCommand)
        {
            // Set fields values
            _executeCommand = executeCommand;
            _canExecuteCommand = canExecuteCommand;
            _executeCommandWithParameter = null;
        }

        /// <summary>
        /// constructor with parameter
        /// </summary>
        /// <param name="executeCommandWithParameter">The action with parameter to execute</param>
        /// <param name="canExecuteCommand">The condition whether to execute a command or not </param>
        public RelayCommand(Action<object> executeCommandWithParameter, Predicate<object> canExecuteCommand)
        {
            // Set fields values
            _executeCommandWithParameter = executeCommandWithParameter;
            _canExecuteCommand = canExecuteCommand;
            _executeCommand?.Invoke();

        }

        /// <summary>
        /// Determines if a command should be executed based on boolean result
        /// </summary>
        /// <param name="parameter">The parameter to evaluate</param>
        /// <returns>A boolean value of True or false</returns>
        public bool CanExecute(object? parameter)
        {
            // Return feedback
            return (parameter is bool).Equals(null) ? true : _canExecuteCommand!(parameter is bool);
        }

        /// <summary>
        /// Executes an action when requested
        /// </summary>
        public void Execute(object? parameter)
        {
            if (parameter is null)
                _executeCommand?.Invoke();
            else
                _executeCommandWithParameter?.Invoke(parameter);
        }
    }
}
