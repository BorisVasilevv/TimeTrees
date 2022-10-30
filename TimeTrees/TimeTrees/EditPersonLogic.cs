using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    public class EditPersonLogic
    {
        //Изменить информацию о пользователе
        public static void EditPersonData(List<Person> people)
        {
            Console.WriteLine("Нажмите любую клавишу, чтобы начать выбор пользователя, данные которого хотите изменить");
            Console.ReadKey(true);
            Console.Clear();
            Person person = FindPerson.Search(people);
            if (person.BirthDate == DateTime.MinValue) return;
            bool answerFit;
            bool editingFinished;
            WriteInstructionsInEditMenu();
            do
            {
                editingFinished = false;
                answerFit = true;
                char answer = Console.ReadKey(true).KeyChar;
                switch (answer)
                {
                    case '1':
                        EditPersonName(person);
                        break;
                    case '2':
                        EditPersonBirthDate(person);
                        break;
                    case '3':
                        EditPersonDeathDate(person);
                        break;
                    case '4':
                        Console.Clear();
                        person.Mother = FindPerson.Parent(person, people);
                        break;
                    case '5':
                        Console.Clear();
                        person.Father = FindPerson.Parent(person, people);
                        break;
                    case '/': return;
                    default:
                        Console.WriteLine("Ответ нераспознан введите ещё раз");
                        answerFit = false;
                        break;
                }

                if (answerFit)
                {
                    Console.WriteLine("Нужно ли изменить что-то ещё? (да/нет)");
                    editingFinished = IsEditingFinished();
                    if (!editingFinished)
                    {
                        Console.Clear();
                        FindPerson.WriteInformation(person);
                        WriteInstructionsInEditMenu();
                    }
                }
            } while ((!answerFit) || !(editingFinished));
        }

        static void WriteInstructionsInEditMenu()
        {
            Console.WriteLine("Меню изменения людей, для отмены введите //back");
            Console.WriteLine("Что вы хотите изменить?");
            Console.WriteLine("1-ФИО, 2-дату рождения, 3-дату смерти, 4-мать, 5-отца, /-выйти в главное меню");
            Console.WriteLine("Введите только цифру  1, или 2, или 3, или 4, или 5, или /");
        }

        static void EditPersonName(Person person)
        {
            Console.WriteLine("Введите новое имя");
            string name = Console.ReadLine();
            if (name == "//back") return;
            person.Name = name;
        }

        static void EditPersonBirthDate(Person person)
        {
            Console.WriteLine("Введите новую дату рождения в формате год-месяц(если известен)-день(если известен)");
            string birthday = SomethingFromUser.GetDate();
            if (birthday == "//back") return;
            DateTime birthDate = DateParser.ParseToDateTime(birthday);
            if ((person.DeathDate == null) || (birthDate.CompareTo(person.DeathDate) < 0))
                person.BirthDate = birthDate;
            else
                Console.WriteLine("Ошибка! Дата смерти не может быть меньше даты рождения");
        }

        static void EditPersonDeathDate(Person person)
        {
            Console.WriteLine("Введите новую дату смерти в формате год-месяц(если известен)-день(если известен)");
            Console.WriteLine("Если человек жив или перестал быть мёртвым просто нажмите Enter");
            string stringBirthDate = StringExtensions.ToString(person.BirthDate);
            string deathDay = SomethingFromUser.GetDeathDate(stringBirthDate);
            if (deathDay == "//back") return;
            if (deathDay == "") person.DeathDate = null;
            else person.DeathDate = DateParser.ParseToDateTime(deathDay);
        }



        static bool IsEditingFinished()
        {
            bool answerIsCorrect = false;
            bool editingFinished = true;
            do
            {
                string secondAnswer = Console.ReadLine();
                secondAnswer = secondAnswer.ToLower();
                if (secondAnswer == "да")
                {
                    answerIsCorrect = true;
                    editingFinished = false;
                }
                else if (secondAnswer == "нет")
                {
                    answerIsCorrect = true;
                    editingFinished = true;
                }
                else
                    Console.WriteLine("Введённый ответ не распознан");
            } while (!answerIsCorrect);
            return editingFinished;
        }
    }
}
