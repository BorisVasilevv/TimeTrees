using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    public class AddPersonLogic
    {
        //Добавление нового человека в базу
        public static void AddPerson(List<Person> people)
        {
            Person newPerson = new Person();
            Console.CursorVisible = true;
            newPerson.Id = people[people.Count - 1].Id+1;

            Console.WriteLine("Чтобы вернуться в главное меню напишите //back");
            Console.WriteLine("Введите имя(ФИО):");
            newPerson.Name = Console.ReadLine();
            if (newPerson.Name == "//back") return;

            Console.WriteLine("Введите дату рожденияв формате год-месяц(если известен)-день(если известен)");
            Console.WriteLine("Примеры ввода дат: 2017-10-25 или 1908-04 или 1204 или 0012");
            string birthDate = SomethingFromUser.GetDate();
            if (birthDate == "//back") return;
            newPerson.BirthDate = DateParser.ParseToDateTime(birthDate);

            Console.WriteLine("Если человек жив просто нажмите Enter");
            //передаём именно birthDate, чтобы тест провести
            string deathDate = SomethingFromUser.GetDeathDate(birthDate);
            if (deathDate == "//back") return;
            if (deathDate != "") newPerson.DeathDate = DateParser.ParseToDateTime(deathDate);

            //Id родителя=0 означает, что информации о родителе нет
            Console.WriteLine("Добавить мать?(да/нет)");
            newPerson.Mother = AddParent(newPerson, people);
            Console.WriteLine("Добавить отца?(да/нет)");
            newPerson.Father = AddParent(newPerson, people);
            people.Add(newPerson);
        }

        //добавить родителя к человеку
        static Person AddParent(Person person, List<Person> people)
        {
            string answer;
            do
            {
                answer = Console.ReadLine();
                if (answer == "нет")
                {
                    continue;
                }
                else if (answer == "да")
                {
                    Person parent = FindPerson.Parent(person, people);
                    return parent;
                }
                else
                {
                    Console.WriteLine("Введённый ответ не распознан, введите ответ ещё раз");
                }
            } while (answer != "нет");
            return null;
        }

        
    }
}
