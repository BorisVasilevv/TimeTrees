using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrees.Core;

namespace TimeTrees
{
    public class AddEventLogic
    {
        //Добавление события
        public static void AddEvent(List<TimelineEvent> timeline, List<Person> people)
        {
            Console.CursorVisible = true;
            TimelineEvent newEvent = new TimelineEvent();

            Console.WriteLine("Чтобы вернуться в главное меню напишите //back");
            Console.WriteLine("Введите дату события");
            Console.WriteLine("В формате год-месяц(если известен)-день(если известен)");
            Console.WriteLine("Примеры ввода дат: 2017-10-25 или 1908-04 или 1204 или 0012");
            string eventDate = SomethingFromUser.GetDate();
            if (eventDate == "//back") return;
            newEvent.Date = DateParser.ParseToDateTime(eventDate);

            Console.WriteLine("Введите название или описание события:");
            string eventNameFromUser = Console.ReadLine();
            if (eventNameFromUser == "//back") return;
            newEvent.NameOfEvent = eventNameFromUser;

            List<Person> participant = AddParticipantsToEvent(newEvent.Date, people);
            newEvent.Participants = participant;
            timeline.Add(newEvent);
            Console.WriteLine("Событие и участник(и) успешно добавлены");
        }

        //Добавление участника события 
        static List<Person> AddParticipantsToEvent(DateTime dateOfEvent, List<Person> people)
        {
            List<Person> participant = new List<Person>();
            string answer = "";
            while (answer != "нет")
            {
                Console.WriteLine($"\nВы добавили {participant.Count} {CorrectEnding.Participant(participant.Count)}");
                Console.WriteLine("Хотите добавить ещё одного участника? (да/нет)");

                answer = Console.ReadLine();
                if (answer == "нет")
                { 
                    continue;
                }
                else if (answer == "да")
                {
                    Console.Clear();
                    Person newParticipant = FindPerson.Search(people);
                    ParticipantTestAndAdd(newParticipant, dateOfEvent, participant);
                }
                else
                {
                    Console.WriteLine("Ответ не распознан");
                }

            }
            return participant;
        }

        //Тест на склеротика (вдруг этот человек уже добавлен к этому событию)
        static bool IsParticipantWasAdd(List<Person> participants, int newId)
        {
            bool testPass = true;
            foreach (Person participant in participants)
                if (participant.Id == newId) testPass = false;
            return testPass;
        }

        static void ParticipantTestAndAdd(Person newParticipant, DateTime dateOfEvent, List<Person> participants)
        {
            if (IsParticipantWasAdd(participants, newParticipant.Id))
            {
                //Тест родился ли этот человек
                //birthday<dateOfEvent (день рождения произошло раньше)
                if (newParticipant.BirthDate.CompareTo(dateOfEvent) < 0)
                {
                    //Тест не умер ли этот человек
                    //deathDate>dateOfEvent или человек жив                    
                    if ((newParticipant.DeathDate == null)
                        || (newParticipant.DeathDate.Value.CompareTo(dateOfEvent) > 0)
                        || (newParticipant.DeathDate.Value == DateTime.MinValue))
                    {
                        participants.Add(newParticipant);
                        Console.WriteLine("\nУчастник успешно добавлен");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка! Этот человек умер раньше, чем произошло событие");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка! Событие произошло раньше, чем человек родился");
                }
            }
            else
            {
                Console.WriteLine("Ошибка! Этот человек уже добавлен");
            }
        }
    }
}
