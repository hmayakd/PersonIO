using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PersonIO
{
    public class Person
    {
        public Person()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
    public class PersonService
    {
        private readonly string path = @"persons";
        public PersonService()
        {
            path = Path.Combine(path, "test.txt");
        }
        public void Create(Person person)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(person.Id.ToString() + "\n"
                    + person.Age.ToString() + "\n"
                    + person.LastName + "\n"
                    + person.FirstName);
            }
        }
        public List<Person> ConvertToPerson(string[] persons)
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < persons.Length; i = i + 4)
            {
                people.Add(new Person()
                {
                    Id = Guid.Parse(persons[i]),
                    Age = Convert.ToInt16(persons[i + 1]),
                    LastName = persons[i + 2],
                    FirstName = persons[i + 3]
                });
            }
            return people;
        }
        public string[] Read()
        {
            string[] persons;
            using (StreamReader reader = new StreamReader(path))
            {
                string textFromFile = reader.ReadToEnd();
                persons = textFromFile.TrimEnd('\n').Split('\n');
            }
            return persons;
        }
        public void Print(List<Person> people)
        {
            foreach (var person in people)
            {
                Console.WriteLine($"ID: {person.Id.ToString()}\n" +
                    $"AGE: {person.Age}\n" +
                    $"LAST NAME: {person.LastName}\n" +
                    $"FIRST NAME: {person.FirstName}\n" +
                    $"{new string('_', 15)}");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            PersonService personService = new PersonService();
            List<Person> people = new List<Person>();
            personService.Create(new Person { Age = 30, LastName = "Einstein", FirstName = "Albert" });
            personService.Create(new Person { Age = 42, LastName = "Newton", FirstName = "Isaac" });
            personService.Create(new Person { Age = 17, LastName = "Lincoln", FirstName = "Abraham" });
            personService.Create(new Person { Age = 20, LastName = "Shakespeare", FirstName = "William" });

            people = personService.ConvertToPerson(personService.Read());
            personService.Print(people);
        }
    }
}
