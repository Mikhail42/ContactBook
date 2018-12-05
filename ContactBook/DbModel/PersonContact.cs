using System.ComponentModel.DataAnnotations.Schema;

namespace ContactBook.DbModel
{
    [Table("PersonContact")]
    public class PersonContact
    {
        [System.ComponentModel.DataAnnotations.Key]
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        [System.ComponentModel.DataAnnotations.Key]
        [ForeignKey("Contact")]
        public int ContactId { get; set; }

        public PersonContact()
        {
        }

        public PersonContact(int personId, int contactId)
        {
            this.PersonId = personId;
            this.ContactId = contactId;
        }

        public Person Person { get; set; }
        public Contact Contact { get; set; }
    }
}
