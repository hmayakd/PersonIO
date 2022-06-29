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
            byte[][] personsByte = new byte[4][];
            personsByte[0] = Encoding.Default.GetBytes(person.Id.ToString() + "\n");
            personsByte[1] = Encoding.Default.GetBytes(person.Age.ToString() + "\n");
            personsByte[2] = Encoding.Default.GetBytes(person.LastName + "\n");
            personsByte[3] = Encoding.Default.GetBytes(person.FirstName + "\n");

            using (FileStream fstream = new FileStream(path, FileMode.Append))
            {
                foreach (byte[] personByte in personsByte)
                {
                    fstream.Write(personByte, 0, personByte.Length);
                }
            }
        }
        public List<Person> ConvertToPerson(string[] persons)
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < persons.Length; i = i + 4)
            {
                try
                {
                    people.Add(new Person()
                    {
                        Id = Guid.Parse(persons[i]),
                        Age = Convert.ToInt16(persons[i + 1]),
                        LastName = persons[i + 2],
                        FirstName = persons[i + 3]
                    });
                }
                catch
                {

                }
            }
            return people;
        }
        public string[] Read()
        {
            string[] persons;
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                persons = textFromFile.Split('\n');
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
