using log4net;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Commands
{
    public class RemovePersonCommand : IAsyncCommand<Model.Person>
    {
        private static readonly ILog log = LogManager.GetLogger("InsertPersonCommand");

        public async Task ExecuteAsync(Model.Person person)
        {
            log.DebugFormat("ExecuteAsync with {0}", person);
            using (var context = new Db.ContactBookContext())
            {
                context.PersonContacts.RemoveRange(context.PersonContacts.Where(x => x.PersonId == person.Id));
                await context.SaveChangesAsync();

                var contactIds = person.Contacts.Where(c => c.Id.HasValue).Select(x => x.Id.Value).OrderBy(x => x).ToArray();
                context.Contacts.RemoveRange(context.Contacts.Where(x => contactIds.Contains(x.Id)));

                context.Persons.Remove(new DbModel.Person(person));

                await context.SaveChangesAsync();
            }
            log.DebugFormat("End of execute");
        }
    }
}
