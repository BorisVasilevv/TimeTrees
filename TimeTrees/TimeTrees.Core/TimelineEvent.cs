using System;

namespace TimeTrees.Core
{
    public class TimelineEvent
    {
        public DateTime Date { get; set; }
        public string NameOfEvent { get; set; }
        public List<Person> Participants { get; set; }
    }
}
