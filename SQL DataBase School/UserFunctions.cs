using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DataBase_School
{
    internal static class UserFunctions
    {
        public static void SwitchFunctions()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("[1] Hämta alla elever");
                Console.WriteLine("[2] Hämta alla elever i en viss klass");
                Console.WriteLine("[3] Lägga till ny personal");
                Console.WriteLine("[4] Hämta personal");
                Console.WriteLine("[5] Hämta alla betyg som satts den senaste månaden");
                Console.WriteLine("[6] Snittbetyg per kurs");
                Console.WriteLine("[7] Lägga till nya elever");
                Console.WriteLine("[8] för att stänga programmet.\n");
                Console.Write("Ange ett nummer: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Case1();
                        break;

                    case "2":
                        Case2();
                        break;

                    case "3":
                        Case3();
                        break;

                    case "4":
                        Case4();
                        break;

                    case "5":
                        Case5();
                        break;

                    case "6":
                        Case6();
                        break;

                    case "7": 
                        Case7();
                        break;

                    case "8":
                        Console.WriteLine("Tryck på [Enter] för att stänga av programmet.");
                        Console.ReadKey();
                        return;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        private static void Case1()
        {
            Console.WriteLine("Ange sorteringsordning [Förnamn] / [Efternamn]");
            string input1 = Console.ReadLine().ToLower();
            Console.WriteLine("Ange ordning [Stigande] / [Fallande]");
            string input2 = Console.ReadLine().ToLower();
            if (input1 == "förnamn")
            {
                if (input2 == "stigande")
                {
                    Functions.ViewAllStudents(true, true);
                }
                else
                {
                    Functions.ViewAllStudents(true, false);
                }
            }
            else if (input1 == "efternamn")
            {
                if (input2 == "stigande")
                {
                    Functions.ViewAllStudents(false, true);
                }
                else
                {
                    Functions.ViewAllStudents(false, false);
                }
            }
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case2()
        {
            Console.WriteLine("Välj en klass genom att ange dess ID");
            int input1 = int.Parse(Console.ReadLine());
            
            Functions.GetStudentsInClass(input1);
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case3()
        {
            Console.WriteLine("Ange uppgifter om den nya anställda");
            Console.Write("Förnamn: ");
            string input1 = Console.ReadLine();
            Console.Write("Efternamn: ");
            string input2 = Console.ReadLine();
            Console.WriteLine("Category som lärare eller rektor.");
            Console.Write("Category: ");
            string input3 = Console.ReadLine();

            Functions.AddNewPersonnel(input1, input2, input3);

            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case4()
        {
            Console.WriteLine("Ange kategori(Alla / Lärare / Annat");
            string input1 = Console.ReadLine();

            Functions.ViewPersonnel(input1);
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case5()
        {
            Functions.GradesThisMonth();
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case6()
        {
            Functions.AverageGrade();
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }

        private static void Case7()
        {
            Console.WriteLine("Ange uppgifter om den nya studenten");
            Console.WriteLine("Förnamn: ");
            string input1 = Console.ReadLine();
            Console.WriteLine("Efternamn: ");
            string input2 = Console.ReadLine();
            Console.WriteLine("Personnummer: (YYYY-MM-DD): ");
            string input3 = Console.ReadLine();
            Console.WriteLine("Class ID: ");
            string input4 = Console.ReadLine();

            if (int.TryParse(Console.ReadLine(), out int classID))
            {
                if (DateTime.TryParse(input3, out DateTime birthDate))
                {
                    Functions.AddNewStudents(input1, input2, birthDate, classID);
                }
                else
                {
                    Console.WriteLine("Fel personnummer");
                }
            }
            else
            {
                Console.WriteLine("Fel Class ID");
            }
            Console.WriteLine("Press [ENTER] to get back to the menu");
            Console.ReadKey();
        }
    }
}
