using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TimeTrees;

namespace TimeTrees.Core
{
    public class WriteDataInFileLogic
    {
        //Запись в файл
        public static void DataInFiles(List<Person> people, List<TimelineEvent> timeline,
            string extension, string peopleFile, string eventsFile)
        {

            if (extension == "csv")
                WriteDataInCsvFile(people, timeline, peopleFile, eventsFile);
            if (extension == "json")
                WriteDataInJsonFile(people, timeline, peopleFile, eventsFile);
        }

        static void WriteDataInCsvFile(List<Person> people, List<TimelineEvent> timeline, string peopleFile, string eventsFile)
        {
            WriteDataInCsvPeopleFile(people, peopleFile);
            WriteDataInCsvEventFile(timeline, eventsFile);
        }

        static void WriteDataInCsvPeopleFile(List<Person> people, string peopleFile)
        {
            string[] lines = new string[people.Count];
            for (int i = 0; i < people.Count; i++)
                lines[i] = StringExtensions.ToString(people[i]);
            File.WriteAllLines(peopleFile, lines);
        }

        static void WriteDataInCsvEventFile(List<TimelineEvent> timeline, string eventsFile)
        {
            string[] lines = new string[timeline.Count];
            for (int i = 0; i < timeline.Count; i++)
            {
                string[] participantIds;
                if (timeline[i].Participants != null)
                {
                    participantIds = new string[timeline[i].Participants.Count];
                    for (int j = 0; j < participantIds.Length; j++)
                    {
                        participantIds[j] = timeline[i].Participants[j].Id.ToString();
                    }
                }
                else
                {
                    participantIds = new string[0];
                }
                string[] information = new string[] { StringExtensions.ToString(timeline[i].Date), timeline[i].NameOfEvent, String.Join(' ', participantIds) };
                lines[i] = String.Join(';', information);
            }
            File.WriteAllLines(eventsFile, lines);
        }

        static void WriteDataInJsonFile(List<Person> people, List<TimelineEvent> timeline, string peopleFile, string eventsFile)
        {
            WriteDataInJsonPeopleFile(people, peopleFile);
            WriteDataInJsonEventFile(timeline, eventsFile);
        }

        static void WriteDataInJsonPeopleFile(List<Person> people, string peopleFile)
        {
            string text = JsonConvert.SerializeObject(people, Formatting.Indented);
            File.WriteAllText(peopleFile, text);
        }

        static void WriteDataInJsonEventFile(List<TimelineEvent> timeline, string eventsFile)
        {
            string text = JsonConvert.SerializeObject(timeline, Formatting.Indented);
            File.WriteAllText(eventsFile, text);
        }

        //Написание файлов, в случае их отсутствия
        public static void WriteTestFilePeopleJson(string file)
        {
            Person[] people = new Person[] { new Person(), new Person() };
            people[0].Id = 1;
            people[0].Name = "Name1";
            people[0].BirthDate = DateTime.Parse("1920-06-05");
            people[0].DeathDate = DateTime.Parse("2016-06-05");
            people[1].Id = 2;
            people[1].Name = "Name2";
            people[1].BirthDate = DateTime.Parse("2016-06-05");
            string text = JsonConvert.SerializeObject(people, Formatting.Indented);
            File.WriteAllText(file, text);
        }

        public static void WriteTestFileEventJson(string file)
        {
            TimelineEvent[] timeline = new TimelineEvent[] { new TimelineEvent(), new TimelineEvent() };
            timeline[0].Date = DateTime.Parse("2000 - 06 - 05");
            timeline[0].NameOfEvent = "какое-то событие 0";
            timeline[1].Date = DateTime.Parse("2001 - 11 - 11");
            timeline[1].NameOfEvent = "какое-то событие12";
            string text = JsonConvert.SerializeObject(timeline, Formatting.Indented);
            File.WriteAllText(file, text);
        }

        public static void WriteTestFilePeopleCsv(string file)
        {
            string[] text = new string[]
                {"1;Високосный Молодой Человек;0;0;2008-06-30",
                "2;Сталин Иосиф Виссарионович;0;0;1879-12-18;1953-03-05",
                "3;Хрущев Никита Сергеевич;0;0;1894-04-17;1971-09-11",
                "4;Брежнев Леонид Ильич;0;0;1906-12-19;1982-11-10" };
            File.WriteAllLines(file, text);
        }

        public static void WriteTestFileEventCsv(string file)
        {
            string[] text = new string[]
                {"1854-10-25;Балаклавское сражение;",
                "1993-10-04;Чёрный октябрь;",
                "1922-10-25;Завершилась Гражданская война в Советской России;" };
            File.WriteAllLines(file, text);
        }
    }
}
