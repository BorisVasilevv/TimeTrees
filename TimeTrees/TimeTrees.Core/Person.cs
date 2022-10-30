using System;

namespace TimeTrees.Core
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
    }
}
