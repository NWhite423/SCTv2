using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SCT
{
    class Functions
    {
        //Internal (private) functions
        private static double Cos(double angle)
        {
            return Math.Cos((Convert.ToDouble(angle) * (Math.PI / 180)));
        }

        private static void AddToQR(string entry, bool endHighlight = true)
        {
            //If there is more entries than the limit, start delete the first (oldest) entry
            if (Program.QuickRefence.Count > Program.QRLimit)
            {
                Program.QuickRefence.RemoveAt(0);
            }
            RefEntry refEntry = new RefEntry
            {
                Message = entry,
                EndHighlight = endHighlight
            };
            Program.QuickRefence.Add(refEntry);
        }

        private static void ShowQR()
        {
            var qrArray = Program.QuickRefence;
            if (qrArray.Count.Equals(0))
            {
                return;
            }
            var highlight = true;
            for (int i = qrArray.Count; i > 0; i--)
            {
                if (highlight)
                {
                    Console.ForegroundColor = Program.ForegrondColor;
                    Console.BackgroundColor = Program.BackgroundColor;
                    Console.WriteLine(qrArray[i-1].Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    if (qrArray[i-1].EndHighlight)
                    {
                        highlight = false;
                    }
                } else
                {
                    Console.WriteLine(qrArray[i-1].Message);
                }
            }
            Console.WriteLine();
        }

        private static void ShowColors(int option)
        {
            Type type = typeof(ConsoleColor);
            if (option.Equals(1))
            {
                foreach (var name in Enum.GetNames(type))
                {
                    Console.BackgroundColor = (ConsoleColor)Enum.Parse(type, name);
                    if (Program.ForegrondColor.Equals((ConsoleColor)Enum.Parse(type, name)))
                    {
                        Console.WriteLine(name + "*");
                    }
                    else
                    {
                        if (name.Equals("White") || name.Equals("Gray") || name.Equals("Yellow"))
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            if (Program.ForegrondColor.Equals(name))
                            {
                                Console.WriteLine(name + "*");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            } else
                            {
                                Console.WriteLine(name);
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        } else
                        {
                            Console.WriteLine(name);
                        }
                    }
                }
            } else
            {
                foreach (var name in Enum.GetNames(type))
                {
                    Console.BackgroundColor = (ConsoleColor)Enum.Parse(type, name);
                    if (Program.BackgroundColor.Equals((ConsoleColor)Enum.Parse(type, name)))
                    {
                        Console.WriteLine(name + "*");
                    }
                    else
                    {
                        if (name.Equals("White") || name.Equals("Gray") || name.Equals("Yellow"))
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            if (Program.BackgroundColor.Equals(name))
                            {
                                Console.WriteLine(name + "*");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.WriteLine(name);
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine(name);
                        }
                    }
                }
                Console.WriteLine();
            }
            
        }

        //Public functions
        public static void SlopeCalc()
        {

        }

        public static void DistanceCalc()
        {

        }

        public static void MeasureDownCalc()
        {
            double RefElevation = 0;
            string input;
            List<string> stringAngle = new List<string> { };

            while (Program.runEvent)
            {
                Console.Clear();
                Console.WriteLine("Measure-down Calculation\n" +
                    "At any time, enter \"E\" to return to the main menu.\n" +
                    "enter \"R\" to reset the top elevation\n" + 
                    "Current Top Elevation: " + RefElevation + "\n");
                ShowQR();

                if (RefElevation.Equals(0))
                {
                    Console.Write("Please enter the elevation: ");
                    input = Console.ReadLine();
                    if (Decimal.TryParse(input, out var output))
                    {
                        var oldNumber = RefElevation;
                        RefElevation = Convert.ToDouble(input);
                        AddToQR(String.Format("Top elevation changed to {0} (was {1})", RefElevation, oldNumber));
                    } else
                    {
                        if (input.Equals("E"))
                        {
                            Program.runEvent = false;
                        } else
                        {
                            AddToQR("Elevation not set, the entry was not a valid number (" + input + ")");
                        }
                    }
                } else
                {
                    Console.Write("Valid entries:\n" +
                        "#.## - Direct measure-down\n" +
                        "#.##@## - measure-down at angle (degrees)\n" +
                        "\n" +
                        "Please enter the measure-down value: ");
                    input = Console.ReadLine();
                    if (input.Equals("") || input.ToUpper().Equals("R"))
                    {
                        RefElevation = 0;
                    } else
                    {
                        if (Decimal.TryParse(input, out var output))
                        {
                            AddToQR(String.Format("{0} - {1} = {2}", RefElevation, input, (RefElevation - Convert.ToDouble(input)).ToString("0.00"), false));
                        } else
                        {
                            if (input.Contains('@'))
                            {
                                stringAngle = input.Split('@').ToList();
                                AddToQR(String.Format("{0} - {1} ({3} * cos(({4})) = {2}", RefElevation, (Convert.ToDouble(stringAngle[0]) * Cos(Convert.ToDouble(stringAngle[1]))).ToString("0.00"), (RefElevation - (Convert.ToDouble(stringAngle[0])*Cos(Convert.ToDouble(stringAngle[1])))).ToString("0.00"), stringAngle[0], stringAngle[1]), false);
                            } else
                            {
                                if (input.ToUpper().Equals("E"))
                                {
                                    Program.runEvent = false;
                                } else
                                {
                                    AddToQR("Error: input was not an accepted value: " + input);
                                }
                            }
                        }
                    }
                }
            }
            Program.runEvent = true;
        }

        public static void Options()
        {
            while (Program.runEvent)
            {
                bool optionComplete = false;
                Console.Clear();
                Console.WriteLine("Please enter an option\n" +
                "Press \"E\" to return to the menu\n" +
                "1. Forground Color \n" +
                "2. Background Color\n" +
                "3. Memory limit\n");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.E:
                        {
                            Program.runEvent = false;
                            break;
                        }
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            while (!optionComplete)
                            {
                                Console.Clear();
                                ShowColors(1);
                                Console.Write("Please enter a color: ");
                                Type type = typeof(ConsoleColor);
                                string input = Console.ReadLine();
                                if (Enum.GetNames(type).Contains(input))
                                {
                                    Program.ForegrondColor = (ConsoleColor)Enum.Parse(type, input);
                                    Settings.Default.ForegroundColor = Program.ForegrondColor;
                                    Settings.Default.Save();
                                    optionComplete = true;
                                } else
                                {
                                    if (input.Equals(""))
                                    {
                                        optionComplete = true;
                                    }
                                }
                            }
                            break;
                        }

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            while (!optionComplete)
                            {
                                Console.Clear();
                                ShowColors(2);
                                Console.Write("Please enter a color: ");
                                Type type = typeof(ConsoleColor);
                                string input = Console.ReadLine();
                                if (Enum.GetNames(type).Contains(input))
                                {
                                    Program.BackgroundColor = (ConsoleColor)Enum.Parse(type, input);
                                    Settings.Default.BackgroundColor = Program.BackgroundColor;
                                    Settings.Default.Save();
                                    optionComplete = true;
                                }
                                else
                                {
                                    if (input.Equals(""))
                                    {
                                        optionComplete = true;
                                    }
                                }
                            }
                            break;
                        }
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            Console.Clear();
                            while (!optionComplete)
                            {
                                Console.WriteLine("\nPlease enter the number of entries you would like to have in the Quick Reference (current " + Program.QRLimit + ": ");
                                string input = Console.ReadLine();
                                if (Decimal.TryParse(input, out var op))
                                {
                                    Program.QRLimit = Convert.ToInt32(input);
                                    Settings.Default.QRLimit = Program.QRLimit;
                                    Settings.Default.Save();
                                    optionComplete = true;
                                }
                                else
                                {
                                    if (input.Equals(""))
                                    {
                                        optionComplete = true;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            Program.runEvent = true;
        }
    }
}
