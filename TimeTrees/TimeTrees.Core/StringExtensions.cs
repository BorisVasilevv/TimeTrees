using System;
using TimeTrees.Core;

namespace TimeTrees.Core
{
    static public class StringExtensions
    {
        static public string ToString(Person person)
        {
            string motherId;
            if ((person.Mother == null) || (person.Mother.BirthDate == DateTime.MinValue))
                motherId = "0";
            else motherId = person.Mother.Id.ToString();

            string fatherId;
            if ((person.Father == null) || (person.Father.BirthDate == DateTime.MinValue))
                fatherId = "0";
            else fatherId = person.Father.Id.ToString();
            //если человек жив дату смерти не пишем
            if (person.DeathDate == null)
                return String.Join(';', new string[]
                {
                    person.Id.ToString(),
                    person.Name.ToString(),
                    motherId,
                    fatherId,
                    StringExtensions.ToString(person.BirthDate)
                });
            else
                return String.Join(';', new string[]
                {
                    person.Id.ToString(),
                    person.Name.ToString(),
                    motherId,
                    fatherId,
                    StringExtensions.ToString(person.BirthDate),
                    StringExtensions.ToString(person.DeathDate.Value)
                });
        }

        static public string ToString(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }

    }

}
