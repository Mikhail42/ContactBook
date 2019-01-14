using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Commands
{
    public class UpdatePersonCommand : IAsyncCommand<Model.Person>
    {
        private static readonly ILog log = LogManager.GetLogger("UpdatePersonCommand");

        public async Task ExecuteAsync(Model.Person person)
        {
            log.DebugFormat("ExecuteAsync: {0}",  person);
            using (var context = new Db.ContactBookContext())
            {
                // update person
                context.Persons.Update(new DbModel.Person(person));

                // update or remove old contacts
                var toDeleteOrUpdateMap = person.Contacts.Where(c => c.Id.HasValue).ToDictionary(x => x.Id.Value);
                var dbContactsMap = (from pc in context.PersonContacts
                                     where pc.PersonId == person.Id
                                     select pc.Contact).ToDictionary(x => x.Id);
                var toDelete = dbContactsMap.Where(kvp => !toDeleteOrUpdateMap.ContainsKey(kvp.Key));
                var toUpdate = toDeleteOrUpdateMap.Where(kvp => dbContactsMap.ContainsKey(kvp.Key))
                    .ToDictionary(k => k.Key, v => v.Value);
                context.PersonContacts.RemoveRange(toDelete.Select(x => new DbModel.PersonContact(person.Id.Value, x.Value.Id)));
                await context.SaveChangesAsync(); // because PersonContact contais foreign key
                context.Contacts.RemoveRange(toDelete.Select(x => x.Value));
                foreach (var v in context.Contacts.Where(c => toUpdate.ContainsKey(c.Id)))
                {
                    context.Entry(v).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                context.Contacts.UpdateRange(toUpdate.Select(x => new DbModel.Contact(x.Value)));

                // add new contacts
                var toAdd = person.Contacts.Where(c => !c.Id.HasValue).Select(c => new DbModel.Contact(c));
                foreach (DbModel.Contact newContact in toAdd)
                {
                    EntityEntry<DbModel.Contact> newContactEntity = await context.Contacts.AddAsync(newContact);
                    await context.SaveChangesAsync();
                    await context.PersonContacts.AddAsync(new DbModel.PersonContact(person.Id.Value, newContactEntity.Entity.Id));
                }

                await context.SaveChangesAsync();
            }
            log.DebugFormat("end of execute");
        }
    }
}
