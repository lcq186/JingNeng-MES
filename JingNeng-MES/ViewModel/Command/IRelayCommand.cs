using System.Windows.Input;

namespace JingNeng_MES.ViewModel
{
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        /// Notifies that the <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)" /> property has changed.
        /// </summary>
        void NotifyCanExecuteChanged();
    }
}