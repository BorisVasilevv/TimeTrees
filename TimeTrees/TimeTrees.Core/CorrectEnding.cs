using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees.Core
{
    public class CorrectEnding
    {
        //Корректируем падеж
        public static (string, string, string) YearsMonthsDays(int year, int month, int day)
        {
            string yearString = CorrectEndingYear(year);
            string mounthString = CorrectEndingMonth(month);
            string dayString = CorrectEndingDay(day);

            return (yearString, mounthString, dayString);
        }

        //Корректируем падеж у количества лет
        public static string CorrectEndingYear(int year)
        {
            string yearString;
            year = year % 100;
            if (((year >= 10) && (year <= 20)) || (year % 10 == 0 || year % 10 >= 5)) yearString = "лет";
            else if (year % 10 == 1) yearString = "год";
            else yearString = "года";
            return yearString;
        }

        //Корректируем падеж у количества дней
        public static string CorrectEndingDay(int day)
        {
            string dayString;
            if (((day >= 10) && (day <= 20)) || (day % 10 == 0 || day % 10 >= 5))  dayString = "дней";
            else if (day % 10 == 1) dayString = "день";
            else dayString = "дня";
            return dayString;
        }

        //Корректируем падеж у количества месяцев
        public static string CorrectEndingMonth(int month)
        {
            string mounthString;
            if (month == 1) mounthString = "месяц";
            else if ((month >= 2) && (month <= 4)) mounthString = "месяца";
            else mounthString = "месяцев";
            return mounthString;
        }

        public static string Participant(int number)
        {
            if ((number % 100 != 11) && (number % 10 == 1)) return "участника";
            else return "участников";
        }
    }
}
