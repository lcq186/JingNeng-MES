using System.Threading.Tasks;
using System.Windows.Input;

namespace JingNeng_MES.ViewModel
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}