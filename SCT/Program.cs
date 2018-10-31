using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCT
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Surveyor Calculation Toolset (V1.2)\n" +
                                "This toolset is designed for quick and effective calculations utilizing the most frequent equations.\n" +
                                "Press \"S\" or 1 to enter slope calculation\n" +
                                "Press \"D\" or 2 to enter the unknown distance calculation\n" +
                                "Press \"M\" or 3 to enter the measure-down calculation\n" +
                                "Press \"O\" for options\n" +
                                "Press \"ESC\" to exit the application\n");
                Console.Write("Please enter your selection now: ");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.S:
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Functions.SlopeCalc();
                            break;
                        }
                    case ConsoleKey.D:
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            Functions.DistanceCalc();
                            break;
                        }
                    case ConsoleKey.M:
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            Functions.MeasureDownCalc();
                            break;
                        }
                    case ConsoleKey.O:
                        {
                            Functions.Options();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Console.WriteLine("\nAre you sure you wish to exit the application? (y/n)");
                            key = Console.ReadKey().Key;
                            switch (key)
                            {
                                case ConsoleKey.Y:
                                    {
                                        Environment.Exit(0);
                                        return;
                                    }
                                case ConsoleKey.N:
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }            
        }
    }
}
