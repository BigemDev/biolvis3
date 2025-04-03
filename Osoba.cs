using System;
using System.Xml.Serialization;

namespace bios_vil3
{
    public class Osoba
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
    }
}