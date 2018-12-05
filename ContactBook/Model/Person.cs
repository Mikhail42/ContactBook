using ContactBook.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ContactBook.Model
{
    public class Person : NotifyPropertyChanged
    {
        public int? Id { get; set; }

        private string name;
        public string Name { get => name; set => SetField(ref name, value); }

        private DateTime? birthday;
        public DateTime? Birthday { get => birthday; set => SetField(ref birthday, value); }

        public ObservableCollection<Contact> Contacts { get; set; } = new ObservableCollection<Contact>();

        private string note;
        public string Note { get => note; set => SetField(ref note, value); }

        public Person()
        {
        }

        public Person(string name, DateTime? birthday, string note, IEnumerable<Contact> contacts, int? id = null)
        {
            this.Id = id;
            this.Name = name;
            this.Birthday = birthday;
            this.Contacts = new ObservableCollection<Contact>(contacts);
            this.Note = note;
        }

        public Person(Person p): this(p.Name, p.Birthday, p.Note, p.Contacts, p.Id)
        {
        }
    }
}
