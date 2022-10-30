using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    public class SomethingFromUser
    {
        //3 метода, которые проверяют корректность даты, полученной от пользователя
        // //back для возможности вернуться в меню
        public static string GetDeathDate(string birthDate)
        {
            bool isBirthDateLessDeathDate;
            bool isFormatCorrect;
            bool isDateLessThenNow;
            string deathDate;
            do
            {
                Console.WriteLine("Введите дату смерти");
                deathDate = Console.ReadLine();
                if ((deathDate == "") || (deathDate == "//back")) return deathDate;

                isFormatCorrect = DateParser.IsDateCorrect(deathDate);
                if (!isFormatCorrect)
                    Console.WriteLine("Некорректный формат даты");

                isDateLessThenNow = (DateParser.ParseToDateTime(deathDate).CompareTo(DateTime.Now) < 0);
                if (isFormatCorrect && !(isDateLessThenNow))
                    Console.WriteLine("Ошибка! Введённая дата ещё не наступила");

                isBirthDateLessDeathDate = DateParser.ParseToDateTime(birthDate).CompareTo(DateParser.ParseToDateTime(deathDate)) < 0;
                if (isFormatCorrect && !isBirthDateLessDeathDate)
                    Console.WriteLine("Ошибка! Дата смерти не может быть меньше даты рождения");


            } while (!isDateLessThenNow || !isBirthDateLessDeathDate || !isFormatCorrect);
            return deathDate;
        }

        public static string GetDate()
        {
            bool isFormatCorrect;
            string someDate;
            do
            {
                someDate = Console.ReadLine();
                if (someDate == "//back") return someDate;
                isFormatCorrect = DateParser.IsDateCorrect(someDate);
                if (!isFormatCorrect)
                    Console.WriteLine("Некоректный формат даты, попробуйте ввести дату ещё раз");
                if ((isFormatCorrect) && !(DateParser.ParseToDateTime(someDate).CompareTo(DateTime.Now) < 0))
                    Console.WriteLine("Ошибка! Введённая дата ещё не наступила, попробуйте ввести дату ещё раз");
            } while ((!isFormatCorrect) || !(DateParser.ParseToDateTime(someDate).CompareTo(DateTime.Now) < 0));
            return someDate;
        }
       
    }
}
