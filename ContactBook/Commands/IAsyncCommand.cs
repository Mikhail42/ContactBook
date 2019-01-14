
using System.Threading.Tasks;

namespace ContactBook.Commands
{
    public interface IAsyncCommand<I>
    {

        Task ExecuteAsync(I input);
    }
}
