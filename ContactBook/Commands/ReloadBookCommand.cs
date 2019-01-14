using log4net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ContactBook.Commands
{
    public class ReloadBookCommand : ICommand<Collection<Model.Person>>
    {
        private static readonly ILog log = LogManager.GetLogger("ReloadBookCommand");

        public void Execute(Collection<Model.Person> book)
        {
            log.Debug("Execute");
            book.Clear();
            IList<Model.Person> newBook = new FindAllPersonsCommand().Execute();
            foreach (Model.Person p in newBook)
            {
                book.Add(p);
            }
            log.Debug("end of execute");
        }
    }
}
