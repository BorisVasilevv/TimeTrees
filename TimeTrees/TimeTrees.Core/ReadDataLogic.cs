using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TimeTrees.Core;


namespace TimeTrees.Core
{
    public class ReadDataLogic
    {
        const int PeopleIdIndex = 0;
        const int PeopleNameIndex = 1;
        const int MotherIdIndex = 2;
        const int FatherIdIndex = 3;
        const int BirthDateIndex = 4;
        const int DeathDateIndex = 5;

        const int DateOfEventIndex = 0;
        const int NameOfEventIndex = 1;
        const int ParticipantIds = 2;

        //Чтение из файлов
        public static List<TimelineEvent> EventsFile(List<Person> people, string format, string eventsFile)
        {
            List<TimelineEvent> timeline = format == "csv"
                ? ReadingEventsCsv(eventsFile, people)
                : ReadEventsJson(eventsFile);
            return timeline;
        }

        public static List<Person> PeopleFile(string format, string peopleFile)
        {
            List<Person> people = format == "csv"
                ? ReadingPeopleCsv(peopleFile)
                : ReadPeopleJson(peopleFile);
            return people;
        }

        //Читаем из файлов Json
        static List<TimelineEvent> ReadEventsJson(string file)
        {
            string text = File.ReadAllText(file);
            //Приводим к читаемому для Json виду
            text = text.Replace("T00:00:00", string.Empty);
            return JsonConvert.DeserializeObject<List<TimelineEvent>>(text);
        }

        static List<Person> ReadPeopleJson(string file)
        {
            string text = File.ReadAllText(file);

            //Приводим к читаемому для Json виду
            text = text.Replace("T00:00:00", string.Empty);
            return JsonConvert.DeserializeObject<List<Person>>(text);
        }

        //Читаем из файлов csv
        static List<TimelineEvent> ReadingEventsCsv(string text, List<Person> people)
        {
            string[] lines = File.ReadAllLines(text);
            List<TimelineEvent> timelineEvent = new List<TimelineEvent>(lines.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                timelineEvent.Add(new TimelineEvent());
                List<Person> participants = new List<Person>();
                string line = lines[i];
                string[] separation = line.Split(";");
                string date = separation[DateOfEventIndex];
                timelineEvent[i].Date = DateParser.ParseToDateTime(date);
                timelineEvent[i].NameOfEvent = separation[NameOfEventIndex];

                string[] ids = separation[ParticipantIds].Split(' ');
                if (ids[0] != "")
                    foreach (string id in ids)
                        foreach (Person person in people)
                            if (person.Id == Int32.Parse(id))
                                participants.Add(person);
                timelineEvent[i].Participants = participants;
            }
            return timelineEvent; 
        }

        static List<Person> ReadingPeopleCsv(string text)
        {
            string[] lines = File.ReadAllLines(text);

            List<Person> people = new List<Person>();

            Person person;
            for (int i = 0; i < lines.Length; i++)
            {
                person = new Person();
                string line = lines[i];
                string[] separation = line.Split(";");
                person.Id = Int32.Parse(separation[PeopleIdIndex]);
                person.Name = separation[PeopleNameIndex];
                person.BirthDate = DateParser.ParseToDateTime(separation[BirthDateIndex]);
                if (separation.Length == 6)
                {
                    person.DeathDate = DateParser.ParseToDateTime(separation[DeathDateIndex]);
                }
                people.Add(person);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] separation = line.Split(";");
                if (Int32.Parse(separation[MotherIdIndex]) != 0)
                {
                    foreach (Person person1 in people)
                        if (person1.Id == Int32.Parse(separation[MotherIdIndex]))
                            people[i].Mother = person1;
                }
                if (Int32.Parse(separation[FatherIdIndex]) != 0)
                {
                    foreach (Person person1 in people)
                        if (person1.Id == Int32.Parse(separation[FatherIdIndex]))
                            people[i].Father = person1;
                }
            }
            return people;
        }
    }
}
