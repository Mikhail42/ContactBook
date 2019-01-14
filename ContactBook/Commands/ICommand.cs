
namespace ContactBook.Commands
{
    public interface ICommand<I>
    {
        void Execute(I input);
    }
}
