using ContactBook.Db;
using ContactBook.Model;
using ContactBook.View;
using log4net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContactBook
{
    public class ContactBookViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(ContactBookViewModel));

        private ObservableCollection<Person> book = new ObservableCollection<Person>();
        private CollectionViewSource bookView;
        public ICollectionView Book { get; set; }

        public void Init()
        {
            log.Info(nameof(Init));
            this.bookView = new CollectionViewSource(); // CollectionViewSource.GetDefaultView(book);
            this.bookView.Source = book;
            this.Book = this.bookView.View;
            ReloadPersonsFromDb();
        }

        public void ShowPerson(Person person, bool changable = false)
        {
            log.DebugFormat("{0}: {1}, changable={2}", nameof(ShowPerson), person, changable);
            PersonViewModel model = new PersonViewModel(person, changable);
            PersonView personView = new PersonView(model);
            personView.Visibility = Visibility.Visible;
            personView.OnPersonViewClose += ReloadPersonsFromDb;
        }

        public void ShowNewPerson()
        {
            log.Debug(nameof(ShowNewPerson));
            var newPerson = new Person();
            this.ShowPerson(newPerson, true);
        }

        public async Task RemovePersonAsync(Person person)
        {
            log.DebugFormat("{0}: {1}", nameof(RemovePersonAsync), person);
            await PersonViewModel.RemovePersonAsync(person);
            ReloadPersonsFromDb();
        }

        private void ReloadPersonsFromDb()
        {
            log.Debug(nameof(ReloadPersonsFromDb));
            book.Clear();
            using (var context = new ContactBookContext())
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
                        ? new List<Contact>()
                        : pl.pcListOrNull.Select(x => new Contact(x.c)).ToList();
                    var person = almostPerson(contacts);
                    //var testContact = new Contact(ContactType.Email, "mytestemail@a.com");
                    //person.Contacts.Add(testContact);
                    book.Add(person);
                }
            }

            log.DebugFormat("end of {0}", nameof(ReloadPersonsFromDb));
        }
    }
}
