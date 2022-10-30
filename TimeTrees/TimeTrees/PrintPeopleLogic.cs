using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    class PrintPeopleLogic
    {
        //Нахождение поджходящих под условие людей и вывод их
        public static void WorkWithPeople(List<Person> people)
        {
            Console.WriteLine("Имена людей, которые родились в високосный год, и их возраст менее 20 лет:");
            for (int i = 0; i < people.Count; i++)
            {
                DateTime birthDay = people[i].BirthDate;
                if ((LeapYear(birthDay.Year)) && (IsAgeLessTwenty(birthDay)))
                    Console.WriteLine(people[i].Name);
            }
        }
        static bool LeapYear(int year)
        {
            return (year % 400 == 0) || ((year % 100 != 0) && (year % 4 == 0));
        }

        static bool IsAgeLessTwenty(DateTime birthDay)
        {
            DateTime today = DateTime.Today;
            if (today.Year - birthDay.Year == 20)
            {
                if ((today.Month < birthDay.Month)) return true;
                else return ((today.Month == birthDay.Month) && (today.Day < birthDay.Day));
            }
            return today.Year - birthDay.Year < 20;
        }
    }
}
