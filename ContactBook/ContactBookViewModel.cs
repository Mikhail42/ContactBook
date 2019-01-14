using ContactBook.Commands;
using ContactBook.Model;
using ContactBook.View;
using log4net;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            await new RemovePersonCommand().ExecuteAsync(person);
            ReloadPersonsFromDb();
        }

        private void ReloadPersonsFromDb()
        {
            new ReloadBookCommand().Execute(book);
        }
    }
}
