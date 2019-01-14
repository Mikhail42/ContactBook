
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
        [Column(name: "Value", TypeName = "NVARCHAR(50)")]
        public string Value { get; set; }

        public Contact(int typeId, string value, int? id = null)
        {
            if (id.HasValue) this.Id = id.Value;
            this.TypeId = typeId;
            this.Value = value;
        }

        public Contact(ContactType type, string value, int? id = null) : this(type.TypeId, value, id)
        {
        }

        public Contact(Model.Contact contact) : this(contact.ContactType, contact.Value, contact.Id)
        {
        }

        public Contact()
        {
        }
    }
}
