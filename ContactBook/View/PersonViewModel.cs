using ContactBook.Db;
using ContactBook.Model;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.View
{
    public class PersonViewModel : Person
    {
        private static readonly ILog log = LogManager.GetLogger("PersonViewModel");

        protected override string ClassName => nameof(PersonViewModel);

        private bool changable = false;
        public bool Changable
        {
            get => changable;
            set
            {
                SetField(ref changable, value);
                this.IsDateEnabled = changable;
                this.IsItemsReadonly = !changable;
            }
        }

        private bool isDateEnabled;
        public bool IsDateEnabled { get => isDateEnabled; set => SetField(ref isDateEnabled, value); }

        private bool isItemsReadonly;
        public bool IsItemsReadonly { get => isItemsReadonly; set => SetField(ref isItemsReadonly, value); }

        public PersonViewModel(Person person, bool changable = false) : base(person)
        {
            this.Changable = changable;
        }

        public async Task SavePersonAsync()
        {
            log.DebugFormat("{0} with Id.HasValue={1}", nameof(SavePersonAsync), this.Id.HasValue);
            if (this.Id.HasValue)
            {
                await UpdateAsync(this);
            }
            else
            {
                await InsertAsync(this);
            }
        }

        public async Task RemovePersonAsync()
        {
            if (this.Id.HasValue)
            {
                await RemovePersonAsync(this);
            }
            else
            {
                throw new InvalidOperationException("Can't remove: Id is null");
            }
        }

        public void AddNewContact()
        {
            log.Debug(nameof(AddNewContact));
            this.Contacts.Add(new Contact());
        }

        public void RemoveContact(Contact contact)
        {
            log.DebugFormat("{0}: {1}", nameof(RemoveContact), contact);
            this.Contacts.Remove(contact);
        }

        public static async Task RemovePersonAsync(Person person)
        {
            log.DebugFormat("{0}: {1}", nameof(RemovePersonAsync), person);
            using (var context = new ContactBookContext())
            {
                context.PersonContacts.RemoveRange(context.PersonContacts.Where(x => x.PersonId == person.Id));
                await context.SaveChangesAsync();

                var contactIds = person.Contacts.Where(c => c.Id.HasValue).Select(x => x.Id.Value).OrderBy(x => x).ToArray();
                context.Contacts.RemoveRange(context.Contacts.Where(x => contactIds.Contains(x.Id)));

                context.Persons.Remove(new DbModel.Person(person));
            
                await context.SaveChangesAsync();
            }
            log.DebugFormat("{0}: {1}", nameof(RemovePersonAsync), "successfully");
        }

        public static async Task UpdateAsync(Person person)
        {
            log.DebugFormat("{0}: {1}", nameof(UpdateAsync), person);
            using (var context = new ContactBookContext())
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
            log.DebugFormat("{0}: {1}", nameof(UpdateAsync), "successfully");
        }

        public static async Task InsertAsync(Person person)
        {
            log.DebugFormat("{0}: {1}", nameof(InsertAsync), person);
            using (var context = new ContactBookContext())
            {
                EntityEntry<DbModel.Person> personEntity =
                    await context.Persons.AddAsync(new DbModel.Person(person));
                await context.SaveChangesAsync();

                person.Id = personEntity.Entity.Id;
                log.DebugFormat("person inserted with id={0}. Try insert contacts", person.Id);
                foreach (Contact c in person.Contacts)
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
