using ContactBook.Util;

namespace ContactBook.Model
{
    public class Contact : NotifyPropertyChanged
    {
        protected override string ClassName => nameof(Contact);

        public int? Id { get; set; }

        public ContactType ContactType { get; private set; }

        private string _value;
        public string Value { get => _value; set => SetField(ref _value, value); }

        public Contact(): this(new ContactType(), "")
        {
        }

        public Contact(ContactType contactType, string value, int? id = null)
        {
            this.Id = id;
            this.ContactType = contactType;
            this.Value = value;
        }

        public Contact(DbModel.Contact c): this(ContactType.GetById(c.TypeId), c.Value, c.Id)
        {
        }

        public override string ToString() => $"Contact[{Id},{this.Value},{this.ContactType}]";
    }
}
