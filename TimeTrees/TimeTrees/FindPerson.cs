using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    class FindPerson
    {

        //Возвращает выбранного человека         
        public static Person Search(List<Person> people)
        {
            int chosenElement = 0;
            string str = "";
            Person person = new Person();
            List<Person> newPeople = people;
            DrawSeachingPeopleMenu(newPeople, chosenElement, str);
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                int amountFoundPeople = newPeople.Count;
                char ch = keyInfo.KeyChar.ToString()[0];
                ConsoleHelper.CleanConsole();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        chosenElement = MenuSelection.ChangeToUp(chosenElement, amountFoundPeople);
                        break;
                    case ConsoleKey.DownArrow:
                        chosenElement = MenuSelection.ChangeToDown(chosenElement, amountFoundPeople);
                        break;
                    case ConsoleKey.Backspace:
                        if (str.Length > 0)
                            str = str.Substring(0, str.Length - 1);
                        newPeople = FindPeople(people, str);
                        chosenElement = 0;
                        break;
                    case ConsoleKey.Enter:
                        //если пользователь жмёт Enter, когда не экране пусто
                        if (newPeople.Count == 0) return new Person();
                        foreach (Person person1 in newPeople)
                        {
                            if (person1.Id == newPeople[chosenElement].Id)
                                person = person1;
                        }
                        break;
                    //если пользователь ввёл букву запускаем поиск
                    default:
                        if (char.IsLetter(ch))
                        {
                            str += ch;
                            //Если пользователь не нашёл нужного человека
                            if (str == "absent") return new Person();
                            newPeople = FindPeople(people, str);
                            chosenElement = 0;
                        }
                        break;
                }
                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    DrawSeachingPeopleMenu(newPeople, chosenElement, str);
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            WriteInformation(person);
            return person;
        }

        //Метод пишуший информацию о выбранном человеке
        public static void WriteInformation(Person choosePerson)
        {
            Console.WriteLine($"Вы выбрали: {choosePerson.Name}");
            Console.WriteLine($"Id: {choosePerson.Id}");
            Console.WriteLine($"Дата рождения: {choosePerson.BirthDate}");
            if (choosePerson.DeathDate == null) Console.WriteLine("жив");
            else Console.WriteLine($"Дата смерти: {choosePerson.DeathDate}");
            string informationAboutMother = SearchParentName(choosePerson.Mother);
            Console.WriteLine($"Мать: {informationAboutMother}");
            string informationAboutFather = SearchParentName(choosePerson.Father);
            Console.WriteLine($"Отец: {informationAboutFather}");
        }

        static string SearchParentName(Person parent)
        {

            string parentInformation;
            if ((parent == null) || (parent.Id == 0))
                parentInformation = "нет информации";
            else
                parentInformation = parent.Name;
            return parentInformation;
        }

        //Метод осуществляет поиск 
        //Выбирает из всех людей тех, которые содержат искомую подстроку
        static List<Person> FindPeople(List<Person> people, string str)
        {
            List<Person> newPeople = new List<Person>();
            foreach (Person person in people)
                if (person.Name.ToLower().Contains(str.ToLower()))
                    newPeople.Add(person);
            return newPeople;
        }

        static void DrawSeachingPeopleMenu(List<Person> people, int idChosenNumber, string str)
        {
            Console.CursorVisible = true;
            Console.WriteLine("Меню поиска людей");
            Console.WriteLine("Если в базе отсутствует человек которого вы искали введите в строку поиска absent");
            string secondLine = "Поиск по ФИО (используйте только буквы). Ваш запрос: " + str;
            Console.WriteLine(secondLine);
            string maxId;
            if (people.Count > 0)
                maxId = people[people.Count - 1].Id.ToString();
            else maxId = "0";
            string maxName = "";
            foreach (Person person in people)
                if (person.Name.Length > maxName.Length)
                    maxName = person.Name;
            int betweenIdAndName = maxId.Length - 1;
            int betweenNameAndBirthDate = maxName.Length - 4;
            string distanseBetweenIdAndName = MakeDistance(betweenIdAndName);
            string distanseBetweenNameAndBirthDate = MakeDistance(betweenNameAndBirthDate);
            Console.WriteLine($"Id{distanseBetweenIdAndName} ФИО {distanseBetweenNameAndBirthDate}Дата рождения \t Дата смерти(если есть)");

            for (int i = 0; i < people.Count; i++)
            {
                maxId = people[people.Count - 1].Id.ToString();
                int deltaIdLength = maxId.Length - people[i].Id.ToString().Length;
                int deltaNameLength = maxName.Length - people[i].Name.Length;
                string distance1 = MakeDistance(deltaIdLength);
                string distance2 = MakeDistance(deltaNameLength);
                string status;

                if (people[i].DeathDate == null) status = "жив";
                else status = people[i].DeathDate.ToString();

                ConsoleColor color = ConsoleColor.Black;
                if (idChosenNumber == i)
                    color = ConsoleColor.DarkCyan;
                Console.WriteLine($"{people[i].Id} {distance1}{people[i].Name}{distance2}" +
                    $" {people[i].BirthDate} \t {status}", Console.BackgroundColor = color);
            }
            Console.SetCursorPosition(secondLine.Length, 2);
        }

        //метод помогает вывести людей в красивом виде            
        static string MakeDistance(int distance)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distance; i++)
                sb.Append(' ');
            return sb.ToString();
        }

        public static Person Parent(Person person, List<Person> people)
        {
            Person parent;
            do
            {
                Console.Clear();
                parent = FindPerson.Search(people); 
            } while (!TestOnDateLogic(person.BirthDate, parent));
            return parent;
        }

        //Тест (умершие и неродившиеся не могут сделать детей)
        public static bool TestOnDateLogic(DateTime birthday, Person parent)
        {
            if ((parent.DeathDate != null) && (parent.DeathDate.Value.CompareTo(birthday) < 0))
            {
                Console.WriteLine("Выбранный человек умер на момент рождения введённого/изменяемого человека");
                Console.WriteLine("Нажмите любую клавишу, чтобы выбрать другого человека в качастве родителя");
                Console.ReadKey(true);
                return false;
            }
            //(parent.birthDate != null) необходимо, чтобы не было ошибки, если пользователь не нашёл нужного человека            
            else if ((parent.BirthDate != DateTime.MinValue) && (birthday.Year - parent.BirthDate.Year < 12))
            {
                Console.WriteLine("Выбранный человек был слишком юн, чтобы быть родителем введённого/изменяемого человека");
                Console.WriteLine("Нажмите любую клавишу, чтобы выбрать другого человека в качастве родителя");
                Console.ReadKey(true);
                return false;
            }
            else return true;
        }
    }
}
