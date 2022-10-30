using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TimeTrees.Core;

namespace TimeTrees
{   
    class Program
    {        
        const int PeopleSearchId   = 0;
        const int SearchEventId    = 1;
        const int AddPeopleId      = 2;
        const int AddEventId       = 3;
        const int EditPeopleDataId = 4;
        const int DeltaDatesId     = 5;
        const int LeapYearBornId   = 6;
        const int ExitId           = 7;


        

        const int AmountOfMainMenuItems = 8;
        
        static void Main(string[] args)
        {
            DirectoryInfo directoryInfo;
            string str = "..\\..\\..\\TimeTrees";
            string[] u = Directory.GetFiles("..\\..\\..\\..\\");

            (string format, string peopleFile, string evetsFile)= FilesHelper.FormatAndFiles();
            List<Person> people = ReadDataLogic.PeopleFile(format, peopleFile);
            List<TimelineEvent> timeline = ReadDataLogic.EventsFile(people, format, evetsFile);
            int chosenId = 0;
            CreateMenu(chosenId);
            do
            {
                Console.CursorVisible = false;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleHelper.CleanConsole();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        chosenId = MenuSelection.ChangeToUp(chosenId, AmountOfMainMenuItems);
                        CreateMenu(chosenId);
                        break;
                    case ConsoleKey.DownArrow:
                        chosenId = MenuSelection.ChangeToDown(chosenId, AmountOfMainMenuItems);
                        CreateMenu(chosenId);
                        break;
                    case ConsoleKey.Enter:
                        if (chosenId == ExitId)
                        {
                            WriteDataInFileLogic.DataInFiles(people, timeline,format,peopleFile,evetsFile);
                            return;
                        }
                        DoAction(chosenId, timeline, people);
                        break;
                    default:
                        CreateMenu(chosenId);
                        break;
                }
            } while (true);
        }

        //Сначала создаём меню
        static void CreateMenu(int idChosenNumber)
        {
            List<MenuItem> menuItem = new List<MenuItem>()
            {
                new MenuItem{Id=PeopleSearchId, Text="Найти человека"},
                new MenuItem{Id=AddPeopleId, Text="Добавить человека"},
                new MenuItem{Id=AddEventId, Text="Добавить событие"},
                new MenuItem{Id=LeapYearBornId, Text="Вывести имена юных людей, родившихся в високосный год"},
                new MenuItem{Id=EditPeopleDataId, Text="Редактировать данные человека"},
                new MenuItem{Id=DeltaDatesId, Text="Вычислить разницу между максимальной и минимальной датами"},
                new MenuItem{Id=ExitId, Text="Выйти"},
                new MenuItem{Id=SearchEventId, Text="Найти событие"}
            };

            for (int i = 0; i < menuItem.Count; i++)
                if (idChosenNumber == menuItem[i].Id)
                    menuItem[i].IsChosen = true;
            DrawMenu(menuItem);
        }

        //Потом рисуем меню
        static void DrawMenu(List<MenuItem> menuItem)
        {
            Console.WriteLine("Главное меню");
            //Цикл внутри цикла нужен чтобы выводить элементы в порядке возрастания id
            for (int i = 0; i < menuItem.Count; i++)
            {
                foreach (var item in menuItem)
                {
                    if (item.Id == i)
                    {
                        if (item.IsChosen)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        }
                        Console.WriteLine(item.Text, Console.BackgroundColor);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
            }
        }   

        //Действия при нажатии Enter
        static void DoAction(int numberOfAction, List<TimelineEvent> timeline, List<Person> people)
        {
            Console.Clear();
            switch (numberOfAction)
            {
                case DeltaDatesId:
                    string str = DeltaDates.WorkWithEvents(timeline);
                    Console.WriteLine(str);
                    break;
                case LeapYearBornId:
                    PrintPeopleLogic.WorkWithPeople(people);
                    break;
                case AddEventId:
                    AddEventLogic.AddEvent(timeline, people);
                    break;
                case AddPeopleId:
                    AddPersonLogic.AddPerson(people);
                    break;
                case PeopleSearchId:
                    FindPerson.Search(people);
                    break;
                case EditPeopleDataId:
                    EditPersonLogic.EditPersonData(people);
                    break;
                case SearchEventId:
                    FindEvent.Search(timeline);
                    break;
            }
            ReturnToMenuOrExit(numberOfAction, timeline, people);
        }

        //Возврат в главное меню
        static void ReturnToMenuOrExit(int choice, List<TimelineEvent> timeline, List<Person> people)
        {
            Console.CursorVisible = true;
            Console.WriteLine("\nХотите продолжить?");
            Console.WriteLine("Введите да/yes, чтобы вернуться в главное меню");
            Console.WriteLine("Введите нет/no, чтобы завершить работу программы");
            bool answerIsCorrect = false;
            do
            {
                string answer = Console.ReadLine();
                answer = answer.ToLower();
                if ((answer == "да") || (answer == "yes"))
                {
                    Console.Clear();
                    CreateMenu(choice);
                    answerIsCorrect = true;
                }
                else if ((answer == "нет") || (answer == "no"))
                {
                    (string format, string peopleFile, string evetsFile) = FilesHelper.FormatAndFiles();
                    WriteDataInFileLogic.DataInFiles(people, timeline,format, peopleFile, evetsFile);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Введённый ответ не распознан");
                }
            } while (!answerIsCorrect);
        }      
    }
}