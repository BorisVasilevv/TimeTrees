using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TimeTrees.Core;

namespace TimeTrees
{
    public class FilesHelper
    {
        static public string Extension;

        public static (string, string, string) FormatAndFiles()
        {
            string peopleFile = "..\\..\\..\\..\\people.";
            string eventsFile = "..\\..\\..\\..\\timeline.";
            if (Extension == null)
            {
                Console.WriteLine("Здравствуйте! С каким форматом файлов Вы хотите работать? (csv/json)");
                string answer;
                do
                {
                    answer = Console.ReadLine().ToLower();
                    if ((answer == "csv") || (answer == "json")) Extension = answer;
                    else Console.WriteLine("Введённый ответ не распознан! Попробуйте ввести ещё раз");
                } while (!(Extension == "csv") && !(Extension == "json"));
                if (Extension == "csv") FileExistenceTestCsv(peopleFile + Extension, eventsFile + Extension);
                else if (Extension == "json") FileExistenceTestJson(peopleFile + Extension, eventsFile + Extension);
                Console.Clear();
            }
            return (Extension, peopleFile + Extension, eventsFile + Extension);
        }

        //Тесты на отсутствие файлов
        public static void FileExistenceTestJson(string peopleJsonFile, string timelineJsonFile)
        {
            if (!File.Exists(peopleJsonFile))
                WriteDataInFileLogic.WriteTestFilePeopleJson(peopleJsonFile);
            if (!File.Exists(timelineJsonFile))
                WriteDataInFileLogic.WriteTestFileEventJson(timelineJsonFile);
        }

        public static void FileExistenceTestCsv(string peopleCsvFile, string timelineCsvFile)
        {
            if (!File.Exists(peopleCsvFile))
                WriteDataInFileLogic.WriteTestFilePeopleCsv(peopleCsvFile);
            if (!File.Exists(timelineCsvFile))
                WriteDataInFileLogic.WriteTestFileEventCsv(timelineCsvFile);
        }

    }
}
