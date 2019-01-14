using log4net;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook.Commands
{
    public class SearchPersonsCommand
    {
        private static readonly ILog log = LogManager.GetLogger("SearchPersonsCommand");
        private const int MAX_DAYS = 10;

        public List<Model.Person> Execute(string toSearch)
        {
            IEnumerable<Model.Person> book = new FindAllPersonsCommand().Execute();
            if (string.IsNullOrWhiteSpace(toSearch)) return book.ToList();

            book = book.Where(p => p.Name.Contains(toSearch)
                    || (p.Note != null && p.Note.Contains(toSearch))
                    || p.Contacts.Any(c => c.Value.Contains(toSearch)));
            return book.ToList();
        }
    }
}
