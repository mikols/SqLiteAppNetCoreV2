using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;

namespace SqLiteAppNetCoreV2
{

    class Program
    {

        public static void Main(string[] args)
        {
            // var rootPath = @"C:\Test";
            List<string> rootPaths = new List<string>();

            while (true)
            {
                DisplayMenu();
                var choice = GetUserChoice();
 
                // Convert 1-based menu choice to 0-based index
                var choiceIndex = (int)choice - 1;
 
                // Check if choice is within the valid range
                if (choiceIndex >= 0 && choiceIndex < Enum
                        .GetValues(typeof(MenuChoices))
                        .Length) // Check against all menu items
                    // Perform action based on user choice index
                    switch (choiceIndex)
                    {
 
                        case (int)MenuChoices.CreateSqliteDatabase:
                            Console.WriteLine("You chose to Create Database.");
                            SqliteDB.CreateSqliteDbTable();
                            break;

                        case (int)MenuChoices.AddDirectories:
                            while (true)
                            {
                                var input = Console.ReadLine();

                                if (input.ToLower() == "done")
                                {
                                    break;
                                }
                                if (Directory.Exists(input))
                                    rootPaths.Add(input);
                                else
                                    Console.WriteLine($"Invalid dir {input} !");
                            }

                            foreach(var dir in rootPaths)
                            {
                                Console.WriteLine(dir);
                            }
                             break;

                        case (int)MenuChoices.FindDirectories:
                            Console.WriteLine("You chose to Find dirs.");
                            foreach(var dir in rootPaths)
                            {
                                Console.WriteLine(dir);
                            }
                            FindDirectories.FindDirs(rootPaths);
                             break;
 

                        case (int)MenuChoices.FindDupes:
                            Console.WriteLine("Find dupes");
                            foreach(var dir in rootPaths)
                            {
                                Console.WriteLine(dir);
                                // FindDupes.FindDuplicateFiles(dir);                                
                                FindDupes.ScanForDuplicateFiles(dir);                                
                            }


                            break;

                        case (int)MenuChoices.ReadTitles:
                            Console.WriteLine("You chose to Read data.");
                            SqliteDB.ReadDataFromSqliteDbTable();
                            break;
 
                        case (int)MenuChoices.FindTitles:
                            Console.WriteLine("You chose to Find Title.");
                            SqliteDB.FindSubstring("SQlite%2");
                            break;

                        case (int)MenuChoices.InsertTitleData:
                            Console.WriteLine("Test to Insert Data.");
                            SqliteDB.TestInsertDataIntoSqliteDbTable();
                            break;
  
                        case (int)MenuChoices.DeleteTitleData:
                            Console.WriteLine("Test to Delete Data.");
                            SqliteDB.DeleteTitleTableData();
                            break;

                        case (int)MenuChoices.ParseDate:
                            Console.WriteLine("Test to Parse Date.");
                            DateExt.TestDateParse();
                            Console.WriteLine("And now...test to Parse Year.");
                            DateExt.TestYearParse();
                            break;

                        case (int)MenuChoices.ParseReso:
                            Console.WriteLine("Test to Parse Res.");
                            DateExt.TestResoParse();
                            break;

                        case (int)MenuChoices.SwapPersons:
                            Console.WriteLine("Test Swap Persons.");
                            TestPersons.Test_Part1();
                            TestPersons.Test_Part2();
                            break;

                        case (int)MenuChoices.Exit:
                            Console.Write(
                                "Are you sure you want to exit the application? (Y/N): "
                            );
                            var confirmation = Console.ReadLine()
                                .ToUpper()[0];
                            Console.WriteLine();
                            if (confirmation == 'Y')
                            {
                                Console.WriteLine("Exiting the application...");
                                return; // Exit the Main method
                            }
 
                            Console.Clear();
                            continue;
 
                        default:
                            Console.WriteLine(
                                "Invalid choice. Please try again."
                            );
                            break;
                    }
                else
                    Console.WriteLine("Invalid choice. Please try again.");
 
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear(); // Clear the console for the next iteration
            }

        }

                /// <summary>
        /// Reads user input from the console and parses it into a
        /// <see cref="T:MenuDemo.MenuChoices" /> enumeration value.
        /// </summary>
        /// <returns>
        /// The <see cref="T:MenuDemo.MenuChoices" /> enumeration value corresponding to
        /// the user input.
        /// If the input cannot be parsed into a valid enumeration value, returns
        /// <see cref="F:MenuDemo.MenuChoices.Unknown" />.
        /// </returns>
        /// <remarks>
        /// This method reads a line of text from the console input and attempts to parse
        /// it into a <see cref="T:MenuDemo.MenuChoices" /> enumeration value.
        /// <para />
        /// If the input matches any of the enumeration values, the corresponding
        /// enumeration value is returned.
        /// <para />
        /// If the input cannot be parsed into a valid enumeration value, the method
        /// returns <see cref="F:MenuDemo.MenuChoices.Unknown" />.
        /// </remarks>
        private static MenuChoices GetUserChoice()
        {
            var input = Console.ReadLine();
            return Enum.TryParse(input, out MenuChoices choice)
                ? choice
                : MenuChoices.Unknown;
        }

 
        /// <summary>
        /// Displays the menu of choices on the console.
        /// </summary>
        /// <remarks>
        /// This method iterates through all the available menu choices and displays them
        /// along with their corresponding numbers.
        /// <para />
        /// The numbering of the choices starts from <c>1</c>.
        /// </remarks>
        private static void DisplayMenu()
        {
            Console.WriteLine("\n===================  SQLite Test ===================\n");
            Console.WriteLine("Please choose an action:\n");
            var menuItemNumber = 1;
            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
                if (choice != MenuChoices.Unknown)
                {
                    var description = GetEnumDescription(choice);
                    Console.WriteLine($"[{menuItemNumber}]:    {description}");
                    menuItemNumber++;
                }
 
            Console.Write("\nEnter your selection: ");
        }


         /// <summary>
        /// Retrieves the description attribute value associated with the specified enum
        /// value.
        /// </summary>
        /// <param name="value">
        /// The <see langword="enum" /> value for which to retrieve the
        /// description.
        /// </param>
        /// <returns>
        /// The description associated with the <see langword="enum" /> value, if
        /// available; otherwise, the
        /// string representation of the <see langword="enum" /> value.
        /// </returns>
        /// <remarks>
        /// This method retrieves the description attribute value, if present, associated
        /// with the specified <see langword="enum" /> value.
        /// <para />
        /// If no description attribute is found, it returns the string representation of
        /// the <see langword="enum" /> value.
        /// </remarks>
        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType()
                             .GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(
                field, typeof(DescriptionAttribute)
            );
            return attribute == null ? value.ToString() : attribute.Description;
        }       
    }
}
