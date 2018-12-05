using ContactBook.Db;
using ContactBook.Model;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace ContactBook.View
{
    public class PersonViewModel : Person
    {
        private static readonly ILog log = LogManager.GetLogger("PersonViewModel");

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

        public PersonViewModel(Person person) : base(person)
        {
            this.Changable = false;
        }

        public void Save()
        {
            if (this.Id.HasValue)
            {
                UpdateAsync(this);
            }
            else
            {
                InsertAsync(this);
            }
        }

        public void Remove()
        {
            if (this.Id.HasValue)
            {
                Remove(this);
            }
            else
            {
                throw new Exception("Can't remove: Id is null");
            }
        }

        public static void Remove(Person person)
        {
            using (var context = new ContactBookContext())
            {
                context.Persons.Remove(new DbModel.Person(person));
            }
        }

        public static async void UpdateAsync(Person person)
        {
            using (var context = new ContactBookContext())
            {
                var personEntity = context.Persons.Update(new DbModel.Person(person));
                foreach (Contact c in person.Contacts)
                {
                    EntityEntry<DbModel.Contact> contactEntity =
                        context.Contacts.Update(new DbModel.Contact(c));
                    var pc = new DbModel.PersonContact(personEntity.Entity.Id, contactEntity.Entity.Id);
                    await context.PersonContacts.AddAsync(pc);
                }
                await context.SaveChangesAsync();
            }
        }

        public static async void InsertAsync(Person person)
        {
            using (var context = new ContactBookContext())
            {
                EntityEntry<DbModel.Person> personEntity =
                    await context.Persons.AddAsync(new DbModel.Person(person));
                person.Id = personEntity.Entity.Id;
                foreach (Contact c in person.Contacts)
                {
                    EntityEntry<DbModel.Contact> contactEntity = 
                        await context.Contacts.AddAsync(new DbModel.Contact(c));
                    var pc = new DbModel.PersonContact(personEntity.Entity.Id, contactEntity.Entity.Id);
                    await context.PersonContacts.AddAsync(pc);
                }
                await context.SaveChangesAsync();
            }
            using (var context = new ContactBookContext())
            {
                var ls = (from pc in context.PersonContacts
                          where pc.Contact.Value != null && pc.Person.FirstName != null
                          select new { pc.Contact.Value, pc.Person.FirstName }
                ).Take(3).ToList();
                var ss = (from p in context.Persons
                          select p.PersonContacts.Select(x => new { x.Contact, x.Person }))
                          .Take(3).ToList();
                var a = 3;
            }
        }
    }
}
