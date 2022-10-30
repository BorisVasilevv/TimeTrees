using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    class FindEvent
    {
        public static void Search(List<TimelineEvent> timeline)
        {
            int chosenElement = 0;
            List<TimelineEvent> newEvents = timeline;
            string str = "";
            DrawSearchEventMenu(newEvents, str, chosenElement);
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                char ch = key.KeyChar.ToString()[0];
                ConsoleHelper.CleanConsole();
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (str.Length > 0)
                            str = str.Substring(0, str.Length - 1);
                        newEvents = FindEvents(timeline, str);
                        chosenElement = 0;
                        break;
                    case ConsoleKey.Enter:
                        WriteInformationAboutEvent(newEvents[chosenElement]);
                        break;
                    case ConsoleKey.DownArrow:
                        chosenElement = MenuSelection.ChangeToDown(chosenElement, newEvents.Count);
                        break;
                    case ConsoleKey.UpArrow:
                        chosenElement = MenuSelection.ChangeToUp(chosenElement, newEvents.Count);
                        break;
                    default:
                        if (char.IsLetter(ch))
                            str += ch;
                        if (str == "absent")
                            return;
                        chosenElement = 0;
                        newEvents = FindEvents(timeline, str);
                        break;
                }
                if (key.Key != ConsoleKey.Enter)
                {
                    DrawSearchEventMenu(newEvents, str, chosenElement);
                }

            } while (key.Key != ConsoleKey.Enter);
        }

        static void DrawSearchEventMenu(List<TimelineEvent> events, string str, int chosenNumber)
        {
            Console.CursorVisible = true;
            Console.WriteLine("Меню поиска событий");
            Console.WriteLine("Если отсутствует событие, которое вы искали введите в строку поиска absent");
            string secondLine = "Поиск по названию. Ваш запрос: " + str;
            Console.WriteLine(secondLine);
            for (int i = 0; i < events.Count; i++)
            {
                if (chosenNumber == i)
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(events[i].NameOfEvent);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(secondLine.Length, 2);
        }

        static List<TimelineEvent> FindEvents(List<TimelineEvent> events, string str)
        {
            List<TimelineEvent> newEvents = new List<TimelineEvent>();
            foreach (TimelineEvent someEvent in events)
                if (someEvent.NameOfEvent.ToLower().Contains(str.ToLower()))
                    newEvents.Add(someEvent);
            return newEvents;
        }

        static void WriteInformationAboutEvent(TimelineEvent timeline)
        {
            Console.Clear();
            Console.WriteLine(timeline.NameOfEvent);
            Console.WriteLine("Произошло в " + timeline.Date.ToString());
            if (timeline.Participants != null)
            {
                Console.WriteLine("Участник(и) события");
                foreach (Person person in timeline.Participants)
                    Console.WriteLine(person.Name);
            }
        }
    }
}
