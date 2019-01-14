using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace ContactBook.Commands
{
    public class InsertPersonCommand : IAsyncCommand<Model.Person>
    {
        private static readonly ILog log = LogManager.GetLogger("InsertPersonCommand");

        public async Task ExecuteAsync(Model.Person person)
        {
            log.DebugFormat("ExecuteAsync: {0}", person);
            using (var context = new Db.ContactBookContext())
            {
                EntityEntry<DbModel.Person> personEntity =
                    await context.Persons.AddAsync(new DbModel.Person(person));
                await context.SaveChangesAsync();

                person.Id = personEntity.Entity.Id;
                log.DebugFormat("person inserted with id={0}. Try insert contacts", person.Id);
                foreach (Model.Contact c in person.Contacts)
                {
                    EntityEntry<DbModel.Contact> contactEntity =
                        await context.Contacts.AddAsync(new DbModel.Contact(c));
                    await context.SaveChangesAsync();

                    var pc = new DbModel.PersonContact(personEntity.Entity.Id, contactEntity.Entity.Id);
                    await context.PersonContacts.AddAsync(pc);
                }

                await context.SaveChangesAsync();
            }
            log.DebugFormat("Successfully insert {0}", person);
        }
    }
}
