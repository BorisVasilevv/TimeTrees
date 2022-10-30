using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees.Core
{
    public class DeltaDates
    {
        //Вычисление разности дат и вывод их на экран
        public static string WorkWithEvents(List<TimelineEvent> timeline)
        {
            (int years, int months, int days) = DeltaMinAndMaxData(timeline);
            (string stringYear, string stringMonth, string stringDay) = CorrectEnding.YearsMonthsDays(years, months, days);
            return $"Между максимальной и минимальной датами прошло: {years} {stringYear}, {months} {stringMonth} и {days} {stringDay}";
        }


        //Вычисление разности между самым поздним и самым ранним событием
        static (int, int, int) DeltaMinAndMaxData(List<TimelineEvent> someEvents)
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            for (int i = 0; i < someEvents.Count; i++)
            {
                if (someEvents[i].Date.CompareTo(maxDate) > 0) //Произошло позже всего
                    maxDate = someEvents[i].Date;
                if (someEvents[i].Date.CompareTo(minDate) < 0) //Произошло раньше всего
                    minDate = someEvents[i].Date;
            }
            int deltaDays = maxDate.Day - minDate.Day;
            int deltaMonths = maxDate.Month - minDate.Month;
            int deltaYears = maxDate.Year - minDate.Year;
            //Проверка на неотрицательность месяцев и дней
            if (deltaDays < 0)
            {
                deltaDays += 31;
                deltaMonths -= 1;
            }
            if (deltaMonths < 0)
            {
                deltaMonths += 12;
                deltaYears -= 1;
            }
            return (deltaYears, deltaMonths, deltaDays);
        }
    }
}
