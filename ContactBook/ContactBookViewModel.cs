using ContactBook.Db;
using ContactBook.Model;
using ContactBook.View;
using log4net;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ContactBook
{
    public class ContactBookViewModel
    {
        private static readonly ILog log = LogManager.GetLogger("ContactBookViewModel");

        public ObservableCollection<Person> Book { get; private set; } = new ObservableCollection<Person>();

        public void Init()
        {
            LoadPersonsFromDb();
        }

        public void ShowPerson(Person person)
        {
            PersonViewModel model = new PersonViewModel(person);
            PersonView personView = new PersonView(model);
            personView.Visibility = Visibility.Visible;
        }

        public void RemovePerson(Person person)
        {
            this.Book.Remove(person);
        }

        private void LoadPersonsFromDb()
        {
            using (var context = new ContactBookContext())
            {
                var pcs = from pc in context.PersonContacts
                          join c in context.Contacts on pc.ContactId equals c.Id
                          group new { pc, c } by pc.PersonId into contactsGroupedByPersonId
                          join p in context.Persons 
                                    on contactsGroupedByPersonId.First().pc.PersonId equals p.Id
                          select new { p, contactsGroupedByPersonId };
                var pcsList = pcs.ToList();
                foreach (var el in pcsList)
                {
                    var contacts = el.contactsGroupedByPersonId.Select(x => new Contact(x.c));
                    var person = el.p.ToPersonViewModel().Invoke(contacts.ToList());
                    Book.Add(person);
                }
            }
        }
    }
}
