using System.Collections.Generic;

namespace ContactBook.Model
{
    public class ContactType
    {
        public string Type { get; }
        public int TypeId { get; }

        private ContactType(string type, int typeId)
        {
            this.TypeId = typeId;
            this.Type = type;
        }

        public static readonly ContactType HomePhone = new ContactType("Домашний", 0);
        public static readonly ContactType MobilePhone = new ContactType("Мобильный", 1);
        public static readonly ContactType WorkPhone = new ContactType("Рабочий", 2);
        public static readonly ContactType Email = new ContactType("e-mail", 3);
        public static readonly ContactType Skype = new ContactType("Skype", 4);

        private static List<ContactType> types = new List<ContactType>()
        {
            HomePhone, MobilePhone, WorkPhone, Email, Skype
        };

        public static IEnumerator<ContactType> GetEnumerator
        {
            get => types.GetEnumerator();
        }

        public static ContactType GetById(int typeId)
        {
            return types.Find(x => x.TypeId == typeId);
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
    }
}
