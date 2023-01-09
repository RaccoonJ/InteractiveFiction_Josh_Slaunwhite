using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace InteractiveFiction
{
    internal class Program
    {
        static string story;
        static string[] storyArray; 
        static string[] currentPage;
        static int pageNum; 

        static void Main()
        {
            Console.SetCursorPosition(0, 0); 
            story = GetStory();
            storyArray = SplitStory();
            WritePage(1);
            Console.ReadKey(true);

            string[] lines = System.IO.File.ReadAllLines(@"story.txt");
        }


        static string GetStory()
        {
            if (FileExistsCheck("!story.txt"))
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Something went wrong");
                Console.ReadKey(true);
                throw new Exception();
            }

            return File.ReadAllText("story.txt");
        }


        static string[] SplitStory() 
        {
            return story.Split('%');
        }


        static void WriteTitle() 
        {
            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine( storyArray[0] ); 
        }


        static void WritePage(int page)
        {
            pageNum = page;

            Console.Clear();
            WriteTitle();

            Console.WriteLine();     
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Page " + pageNum); 

            currentPage = storyArray[page].Split('$'); 

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string s in currentPage[0].Split('&')) 
                Console.WriteLine(s);

            if (GameOverText())
            {
                Restart();
            }
            ShowChoices();
            PickPage();
        }


 
        static void ShowChoices() 
        {
            for (int s = 0; s < currentPage[1].Split('&').Length; s++) 
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Page " + currentPage[2].Split('&')[s] + ": ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(currentPage[1].Split('&')[s]);
            }

            Console.WriteLine();
            Console.WriteLine(" -------------");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("  \"s\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to Save ");
            Console.WriteLine();
            Console.WriteLine(" -------------");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("  \"l\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to Load");
            Console.WriteLine();
            Console.WriteLine(" -------------");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("  \"q\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" to Quit");
            Console.WriteLine();
            Console.WriteLine(" -------------");
            Console.WriteLine();
            Console.Write("Input Page Number and press Enter: ");
        }


        static bool GameOverText() 
        {
            if (currentPage[1][0] == '!') 
            {
                for (int i = 0; i < currentPage[1].Length; i++)
                    Console.Write('-');
                Console.WriteLine();

                Console.WriteLine(currentPage[1].Remove(0, 1));

                for (int i = 0; i < currentPage[1].Length; i++)
                    Console.Write('-');

                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        static void PickPage()
        {
            string input = Console.ReadLine();
            Console.WriteLine(input);

            if (input.ToLower() == "s")
            {
                Save();
                WritePage(pageNum);
            }
            else if (input.ToLower() == "l")
            {
                Load();
            }
            else if (input.ToLower() == "q")
            {
                Restart();
            }
            else
            {
                foreach (string s in currentPage[2].Split('&'))
                {
                    if (input == s)
                    {
                        WritePage(Int32.Parse(s));
                    }
                }
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Not an option");
                Console.ReadKey(true);
                WritePage(pageNum);
            }
        }


        static void Save() 
        {
            File.WriteAllText("savegame.txt", pageNum.ToString());
        }


        static void Load() 
        {
            try
            {
                WritePage(Int32.Parse(File.ReadAllText("savegame.txt")));
            }
            catch (Exception) 
            {
                WritePage(pageNum);
            }

        }

        static void Restart()
        {
            WritePage(1);
        }

        static bool FileExistsCheck(string fileCheck)
        {
            return File.Exists(fileCheck);
        }
    }
}
