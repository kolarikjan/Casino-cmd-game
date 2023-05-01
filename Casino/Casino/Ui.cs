using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Ui
    {
        public static void MenuLine()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------");
            Console.WriteLine();
        }
        public void MenuHeader(string title)
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write("Casino");
            Console.Write(" - " + title);
            Console.WriteLine();
        }
        public int GenerateMenu(string title = "", bool generateHeader = true)
        {
            int run = 0;
            while (true)
            {
                if (generateHeader)
                {
                    this.MenuHeader(title);
                    Ui.MenuLine();
                }
                int menuOptions = 0;
                if (run == 0 || generateHeader)
                {
                    switch (title)
                    {
                        case "Hlavní menu":
                            menuOptions = 3;
                            Console.WriteLine("1 - Hrát hazardní hry\n");
                            Console.WriteLine("2 - Správa profilů\n");
                            Console.WriteLine("3 - Ukončit aplikaci");
                            break;
                        case "Herní menu":
                            menuOptions = 3;
                            Console.WriteLine("1 - Blackjack\n");
                            Console.WriteLine("2 - Ruleta\n");
                            Console.WriteLine("3 - Hod mincí\n");
                            Console.WriteLine("4 - Ukončit aplikaci");
                            break;
                        case "Hra probíhá | Blackjack":
                            menuOptions = 3;
                            Console.WriteLine("1 - Hit\n");
                            Console.WriteLine("2 - Stand\n");
                            Console.WriteLine("3 - Double\n");
                            Console.WriteLine("4 - Vzdát hru");
                            break;
                        case "Konec hry":
                            menuOptions = 3;
                            Console.WriteLine("1 - Zkusit znovu\n");
                            Console.WriteLine("2 - Zpět do herního menu");
                            break;
                        case "Výběr barvy | Ruleta":
                            menuOptions = 4;
                            Console.WriteLine("1 - Červená\n");
                            Console.WriteLine("2 - Červná\n");
                            Console.WriteLine("3 - Zelená\n");
                            Console.WriteLine("4 - Zrušit sázku");
                            break;
                        default:
                            Console.WriteLine("Zatím tady nic není");
                            break;
                    }
                    MenuLine();
                }
                else
                {
                    Console.WriteLine();
                }
                Console.Write("Zadejte akci: ");
                try
                {
                    string? userInput = Console.ReadLine();
                    int action = 0;
                    if (userInput != null)
                    {
                        action = int.Parse(userInput);
                        if (action > 0 && action <= menuOptions)
                        {
                            return action;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
                run++;
            }

        }
    }
}
