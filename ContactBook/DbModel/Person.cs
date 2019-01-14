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
        [Column(name: "Id")]
        public int Id { get; set; }
        [Column(name: "Name", TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }
        [Column(name: "Note", TypeName = "NVARCHAR(400)")]
        public string Note { get; set; }
        [DataType(DataType.DateTime)]
        [Column(name: "Birthday")]
        public DateTime? Birthday { get; set; }

        public Person()
        {
        }

        public Person(string name, DateTime? birthday = null, string note = null, int? id = null)
        {
            this.Name = name;
            this.Birthday = birthday;
            this.Note = note;
            if (id.HasValue) this.Id = id.Value;
        }

        public Person(Model.Person p)
        {
            this.Name = p.Name;
            this.Birthday = p.Birthday;
            this.Note = p.Note;
            if (p.Id.HasValue) this.Id = p.Id.Value;
        }

        public Func<IEnumerable<Model.Contact>, Model.Person> ToPersonViewModel()
        {
            return (contacts) => new Model.Person(this.Name, this.Birthday, this.Note, contacts, Id);
        }

        public List<PersonContact> PersonContacts { get; set; }
    }
}