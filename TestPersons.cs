using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SqLiteAppNetCoreV2
{
    public class TestPersons
    {
        public static void Test_Part2()
        {
            Console.WriteLine("---- Test Persons Swap . Part 2 ----");

            Console.WriteLine("Test with 3 persons");
            var persons = new PersonHandler();
            persons.TargetIsFile = true;
            persons.SetPersons("Goofy");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Mickey Mouse");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Pluto");
            var result = persons.GetPersons();
            Console.WriteLine("-->" + result);

            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);
            
            // Test with 2 persons
            Console.WriteLine("\nTest with 2 persons");
            persons.Clear();
            persons.SetPersons("Lando.Norris");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Oscar.Piastri");
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);

           // Test with 1 persons
            Console.WriteLine("\nTest with 1 persons");
            persons.Clear();
            persons.SetPersons("One.Person");
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.RotatePersons();
            result = persons.GetPersons();
            Console.WriteLine(result);

        }

        public static void Test_Part1()
        {
            Console.WriteLine("---- Test Persons Swap . Part 1 ----");

            var persons = new PersonHandler();
            persons.TargetIsFile = true;

            // Test with 3 persons
            persons.SetPersons("Donald.Duck");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Mickey.Mouse");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Uncle.Scrooge");
            var result = persons.GetPersons();
            Console.WriteLine(result );

            persons.SwapPersons();
            result = persons.GetPersons();
            Console.WriteLine(result);

            persons.SwapPersons();
            result = persons.GetPersons();
            Console.WriteLine(result);

            // Test with 2 persons
            persons.Clear();
            persons.SetPersons("Lando.Norris");
            persons.AddPersonSeparatorToPersonsList();
            persons.SetPersons("Oscar.Piastri");
            result = persons.GetPersons();
            Console.WriteLine(result);
            persons.SwapPersons();
            result = persons.GetPersons();
            Console.WriteLine(result);
        }
    }


    public class PersonHandler
    {
        public bool TargetIsFile { get => _targetIsFile; set => _targetIsFile = value; }
        private bool _targetIsFile = false;
        public string Persons => GetPersons(PersonSeparator(_targetIsFile), NameSeparator(_targetIsFile));

        public string PersonSeparator(bool targetIsFile) => targetIsFile ? MyConstants.AND_SEPARATORPERSONFILE : ", ";
        public string NameSeparator(bool targetIsFile) => targetIsFile ? "." : " ";

        public bool HasPersons { get => (_Persons.Count > 0) && (_Persons[0].Length > 0); }
        private List<string> _Persons = new List<string>();

        public void Clear()
        {
            _Persons.Clear();
        }

        public void SwapPersons()
        {
            // Swap 2 persons with firstname and lastname
            if (_Persons.Count == 5)
            {
                _Persons.Swap(0, 3);
                _Persons.Swap(1, 4);
            }
            
            // Swap 3 persons with firstname and lastname
            if (_Persons.Count == 8)
            {
                _Persons.Swap(0, 6);
                _Persons.Swap(1, 7);

                _Persons.Swap(6, 3);
                _Persons.Swap(7, 4);
            }
        }


        public void RotatePersons()
        {
            var personsStr = GetPersons();
            _Persons.Clear();

            string[] separators = { PersonSeparator(_targetIsFile) };
            var list = personsStr.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (list.Length > 1)
            {
                string lastElement = list[list.Length - 1];
                for (int i = list.Length - 1; i > 0; i--)
                {
                    list[i] = list[i - 1];
                    SetPersons(list[i - 1]);
                    AddPersonSeparatorToPersonsList();
                }
                list[0] = lastElement;
                SetPersons(lastElement);
            }

        }


        public string GetPersons()
        {
            return GetPersons(",", " ");
        }

        public string GetPersons(string personSeparator, string nameSeparator)
        {
            var persons = "";
            foreach (var personName in _Persons)
            {
                if (personName.Trim().Equals(MyConstants.NAME_SEPARATOR))
                {
                    persons = persons.Trim() + personSeparator;
                }
                else
                {
                    persons += personName.Trim() + nameSeparator;
                }
            }
            // Console.WriteLine("Nr Names: " + _Persons.Count.ToString()) ;
            return persons.Trim();
        }

        public void SetPersons(string personStr)
        {
            if (string.IsNullOrEmpty(personStr))
                return;
                // Console.WriteLine("1. " + personStr + "MyConstants.NAME_SEPARATOR: " + MyConstants.NAME_SEPARATOR);
            personStr = personStr.Replace(",", "." + MyConstants.NAME_SEPARATOR + ".");
                // Console.WriteLine("2. " + personStr);
            personStr = personStr.Replace("+", "." + MyConstants.NAME_SEPARATOR + ".");
                // Console.WriteLine("3. " + personStr);
            char[] separators = { ' ', ',', '.' };
            var personStrs = personStr.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach(var personName in personStrs)
            {
                AddPersonName(personName);
            }
        }

        
        public void AddPersonSeparatorToPersonsList()
        {
            _Persons.Add(MyConstants.NAME_SEPARATOR);
        }

        public void AddPersonName(string name)
        {
            name = name.Trim();
            if (!string.IsNullOrEmpty(name) /* && name.IsValidName() */)
            {
                // if (name.ToUpper() == "von")
                    _Persons.Add(name.Trim());
                // else
                    // _Persons.Add(name.Trim().UppercaseFirstLetter());
            }
        }


    }

    public static class PersonsExt
    {
 
        public static void Rotate<T>(this List<T> list)
        {
            if (list.Count > 1)
            {
                T lastElement = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                list.Insert(0, lastElement);
            }
        }

        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
       
    }

    public static class MyConstants
    {
        public static string _AND = "And";

        public static string NAME_SEPARATOR => "NAMESEP";
        public static string AND_SEPARATOR => ".and.";
        public static string AND_SEPARATORPERSONFILE => ",";

        }
}