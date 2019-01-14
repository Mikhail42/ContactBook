using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBook.Commands
{
    public class FindContactsByBirthdayCommand
    {
        private static readonly ILog log = LogManager.GetLogger("FindContactsByBirthdayCommand");
        private const int MAX_DAYS = 10;

        public List<Model.Person> Execute(DateTime date)
        {
            IEnumerable<Model.Person> book = new FindAllPersonsCommand().Execute();
            book = book.Where(p => p.Birthday.HasValue
                    && p.Birthday.Value.DayOfYear > date.DayOfYear
                    && p.Birthday.Value.DayOfYear < date.DayOfYear + MAX_DAYS);
            return book.ToList();
        }
    }
}
