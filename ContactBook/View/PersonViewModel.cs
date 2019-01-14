using ContactBook.Commands;
using ContactBook.Model;
using log4net;
using System;
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
                await new UpdatePersonCommand().ExecuteAsync(this);
            }
            else
            {
                await new InsertPersonCommand().ExecuteAsync(this);
            }
        }

        public async Task RemovePersonAsync()
        {
            if (this.Id.HasValue)
            {
                await new RemovePersonCommand().ExecuteAsync(this);
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
    }
}
