
using ContactBook.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactBook.DbModel
{
    [Table(name: "Contact")]
    public class Contact
    {
        [Column(name: "Id")]
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        [Column(name: "Type")]
        public int TypeId { get; set; }
        [Column(name: "Value")]
        public string Value { get; set; }

        public Contact(int typeId, string value)
        {
            this.TypeId = typeId;
            this.Value = value;
        }

        public Contact(ContactType type, string value): this(type.TypeId, value)
        {
        }

        public Contact(Model.Contact contact) : this(contact.ContactType, contact.Value)
        {
        }

        public Contact()
        {
        }
    }
}
