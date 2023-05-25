using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JingNeng_MES.ViewModel
{
    public sealed class RelayCommand : IRelayCommand, ICommand
    {
        /// <summary>
        /// The <see cref="T:System.Action" /> to invoke when <see cref="M:CommunityToolkit.Mvvm.Input.RelayCommand.Execute(System.Object)" /> is used.
        /// </summary>
        private readonly Action execute;
        /// <summary>
        /// The optional action to invoke when <see cref="M:CommunityToolkit.Mvvm.Input.RelayCommand.CanExecute(System.Object)" /> is used.
        /// </summary>
        private readonly Func<bool>  canExecute;

        /// <inheritdoc />
        public event EventHandler  CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunityToolkit.Mvvm.Input.RelayCommand" /> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="execute" /> is <see langword="null" />.</exception>
        public RelayCommand(Action execute)
        {
      
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommunityToolkit.Mvvm.Input.RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if <paramref name="execute" /> or <paramref name="canExecute" /> are <see langword="null" />.</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
    
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc />
        public void NotifyCanExecuteChanged()
        {
            EventHandler canExecuteChanged = this.CanExecuteChanged;
            if (canExecuteChanged == null)
                return;
            canExecuteChanged((object)this, EventArgs.Empty);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object   parameter)
        {
            Func<bool> canExecute = this.canExecute;
            return canExecute == null || canExecute();
        }

        /// <inheritdoc />
        public void Execute(object  parameter) => this.execute();
    }
}