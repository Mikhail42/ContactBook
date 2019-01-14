using ContactBook.Commands;
using ContactBook.Model;
using ContactBook.Util;
using ContactBook.View;
using log4net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContactBook
{
    public class ContactBookViewModel : NotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger(nameof(ContactBookViewModel));

        private ObservableCollection<Person> book = new ObservableCollection<Person>();
        private CollectionViewSource bookView;
        public ICollectionView Book { get; set; }

        private bool isActiveSoonBirthday;
        public bool IsActiveSoonBirthday { get => isActiveSoonBirthday; set => SetField(ref isActiveSoonBirthday, value); }

        protected override string ClassName => nameof(ContactBookViewModel);
        
        public void Init()
        {
            log.Info(nameof(Init));
            this.bookView = new CollectionViewSource();
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

        public void ReloadPersonsFromDb()
        {
            IsActiveSoonBirthday = false;
            new ReloadBookCommand().Execute(book);
        }

        public void ShowPersonsWithSoonBirthday()
        {
            IEnumerable<Person> persons = new FindContactsByBirthdayCommand().Execute(System.DateTime.Now);
            this.book.Clear();
            foreach (var p in persons)
            {
                this.book.Add(p);
            }
        }
    }
}
