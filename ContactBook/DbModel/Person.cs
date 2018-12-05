using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactBook.DbModel
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public string Note { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Birthday { get; set; }

        public Person()
        {
        }

        public Person(string firstName, string lastName, DateTime? birthday = null, 
            string note = null, string midleName = null, int? id = null)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MidleName = midleName;
            this.Birthday = birthday;
            this.Note = note;
            if (id.HasValue) this.Id = id.Value;
        }

        public Person(Model.Person p)
        {
            string[] name = p.Name.Trim().Split(' ');
            FirstName = name[0];
            if (name.Length > 2)
            {
                MidleName = name[1];
                LastName = name[2];
            }
            else
            {
                LastName = name[1];
            }
            this.Birthday = p.Birthday;
            this.Note = p.Note;
            if (p.Id.HasValue) this.Id = p.Id.Value;
        }

        public Func<IEnumerable<Model.Contact>, Model.Person> ToPersonViewModel()
        {
            string name = (this.FirstName + " " + this.MidleName + " " + this.LastName).Replace("  ", " "); 
            return (contacts) => new Model.Person(name, this.Birthday, this.Note, contacts, Id);
        }

        public List<PersonContact> PersonContacts { get; set; }
    }
}
