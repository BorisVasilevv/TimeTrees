using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees.Core
{
    public class DateParser
    {
        //Переводим из строки в дату
        //Если строка была только из года добавляем к ней месяц
        //Иначе не будет парситься
        public static DateTime ParseToDateTime(string someDate)
        {
            if (someDate.Length <= 4)
                someDate += "-01";
            return DateTime.Parse(someDate);
        }

        public static bool IsDateCorrect(string someDate)
        {
            //есть ли между цифрами тире
            string[] date = someDate.Split('-', '.');
            int[] testArray = new int[date.Length];
            try
            {
                for (int i = 0; i < testArray.Length; i++)
                {
                    //являются ли введённые значения цифрами
                    testArray[i] = int.Parse(date[i]);
                }
            }
            catch
            {
                return false;
            }
            //для формата даты yyyy-MM-dd
            //Год должен состоять из 3 или 4 цифр //В дате не более 3 элементов
            try
            {
                bool a = (testArray[0] >= 100 && testArray[0] <= DateTime.Now.Year) && (date.Length == 1) ||
                    (testArray.Length == 2) && (testArray[1] <= 12 && testArray[1] != 0) || //Месяц не более 12-ого
                        (testArray.Length == 3 || (testArray[2] <= 31 || testArray[2] != 0)); //День не более 31-ого
                if (a) return a;
            }
            catch
            {
                return false;
            }
            //для формата даты dd-MM-yyyy
            bool b = (date.Length <= 3)
                && testArray.Length == 1 && (testArray[0] >= 100 && testArray[0] <= DateTime.Now.Year)
                || (testArray.Length == 2 && (testArray[1] >= 100 && testArray[1] <= DateTime.Now.Year)
                && (testArray[0] <= 12 && testArray[0] != 0)) //Месяц не более 12-ого
                || ((testArray[2] >= 100 && testArray[2] <= DateTime.Now.Year) 
                && (testArray[1] <= 12 && testArray[1] != 0) //Месяц не более 12-ого
                && (testArray[0] <= 31 || testArray[0] != 0));  //День не более 31-ого
            return b;
        }
    }
}
