using ContactBook.Util;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook.Model
{
    public class ContactType : NotifyPropertyChanged
    {
        protected override string ClassName => nameof(ContactType);

        private string type;
        public string Type
        {
            get => type;
            set { if (SetField(ref type, value)) this.TypeId = GetByName(type).TypeId; }
        }

        private int typeId = -1;
        public int TypeId { get => typeId; set => SetField(ref typeId, value); }

        private ContactType(string type, int typeId)
        {
            this.typeId = typeId;
            this.type = type;
        }

        public ContactType()
        {
        }

        public static readonly ContactType HomePhone = new ContactType("Домашний", 0);
        public static readonly ContactType MobilePhone = new ContactType("Мобильный", 1);
        public static readonly ContactType WorkPhone = new ContactType("Рабочий", 2);
        public static readonly ContactType Email = new ContactType("e-mail", 3);
        public static readonly ContactType Skype = new ContactType("Skype", 4);

        private static IList<ContactType> types { get; } = new List<ContactType> { HomePhone, MobilePhone, WorkPhone, Email, Skype };
        private static IList<string> typeNames { get; } = types.Select(x => x.Type).ToList();

        public IList<string> Types { get => typeNames; }

        public static IEnumerator<ContactType> GetEnumerator
        {
            get => types.GetEnumerator();
        }

        public static ContactType GetByName(string type)
        {
            return types.FirstOrDefault(x => x.Type == type);
        }

        public static ContactType GetById(int typeId)
        {
            return types.FirstOrDefault(x => x.TypeId == typeId);
        }

        override public bool Equals(object another)
        {
            return (another is ContactType) && (Type == (another as ContactType)?.Type);
        }

        override public int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public static bool operator ==(ContactType type1, ContactType type2)
        {
            return type1?.Type == type2?.Type;
        }

        public static bool operator !=(ContactType type1, ContactType type2)
        {
            return !(type1 == type2);
        }

        public override string ToString() => this.Type;
    }
}
