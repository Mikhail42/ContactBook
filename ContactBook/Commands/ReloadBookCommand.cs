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
            using (var context = new Db.ContactBookContext())
            {
                var personContacts = from pc in context.PersonContacts
                                     join c in context.Contacts on pc.ContactId equals c.Id
                                     group new { pc, c } by pc.PersonId into contactsGroupedByPersonId
                                     select contactsGroupedByPersonId;
                var personList = from p in context.Persons
                                 orderby p.FirstName
                                 join pc in personContacts
                                 on p.Id equals pc.First().pc.PersonId into pcList
                                 from pcListOrNull in pcList.DefaultIfEmpty()
                                 select new { p, pcListOrNull };
                foreach (var pl in personList)
                {
                    var almostPerson = pl.p.ToPersonViewModel();
                    var contacts = (pl.pcListOrNull == null)
                        ? new List<Model.Contact>()
                        : pl.pcListOrNull.Select(x => new Model.Contact(x.c)).ToList();
                    var person = almostPerson(contacts);
                    book.Add(person);
                }
            }

            log.Debug("end of execute");
        }
    }
}
