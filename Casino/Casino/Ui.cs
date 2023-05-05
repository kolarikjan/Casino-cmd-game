using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Ui
    {
        //
        // trida pro uzivatelske rozhrani, vypisovani "grafickych" casti aplikace jako menu, header apod
        //

        public static void MenuLine()
        {
            //
            // pouze kosmeticka funkce vyvarejici caru, tak aby byla vzdy stejna
            //
            Console.WriteLine();
            Console.WriteLine("--------------------");
            Console.WriteLine();
        }
        public void MenuHeader(string title)
        {
            //
            // funkce generujici hlavicku pro jednotlive stavy aplikace
            //
            // title = jaky text se vlozi do headeru - napr (o jake meno se jedna, v jakem stadiu jsou rozehrane hry apod)
            //
            Console.Clear();
            Console.WriteLine("");
            Console.Write("Casino");
            Console.Write(" - " + title);
            Console.WriteLine("");
        }
        public int GenerateMenu(string title = "", bool generateHeader = true)
        {
            //
            // funkce generujici menu, stara se o vypis moznosti jednotlivych menu a overeni volby uzivatele a nasledne navraceni volby
            //
            // title = urcuje jake menu se vypise, vsechna menu jsou vypsana ve switchni nize
            //         jaky text se vlozi do headeru - napr (o jake meno se jedna, v jakem stadiu jsou rozehrane hry apod)
            // generateHeader = jestli se ma vypsat hlavicka a hned potom menu (v pripade true)
            //         false se zadava rucne jen v pripade, ze mezi menu a hlavicku chceme vlozit nejakou dalsi zpravu, napr vysledek hry
            //         v tomto pripade se musi samostatne vyvolat MenuHeader(), pote nase zprava a pote GenerateMenu(xxx, false)
            //
            int run = 0;
            int menuOptions = 0;
            while (true)
            {
                if (generateHeader)
                {
                    this.MenuHeader(title);
                    Ui.MenuLine();
                }
                if (run == 0 || generateHeader)
                {
                    switch (title)
                    {
                        // v pripade vytvareni noveho menu, je nutne vytvorit novou case (pokud nechceme vyuzit jiz zde vytvorene)
                        case "Hlavní menu":
                            menuOptions = 3;
                            Console.WriteLine("1 - Hrát hazardní hry\n");
                            Console.WriteLine("2 - Správa profilů\n");
                            Console.WriteLine("3 - Ukončit aplikaci");
                            break;
                        case "Herní menu":
                            menuOptions = 4;
                            Console.WriteLine("1 - Blackjack\n");
                            Console.WriteLine("2 - Ruleta\n");
                            Console.WriteLine("3 - Hod mincí\n");
                            Console.WriteLine("4 - Zpět do hlavního menu");
                            break;
                        case "Správa profilů":
                            menuOptions = 4;
                            Console.WriteLine("1 - Vytvořit nový profil\n");
                            Console.WriteLine("2 - Přepnout profil\n");
                            Console.WriteLine("3 - Smazat všechny uložené profily\n");
                            Console.WriteLine("4 - Zpět do hlavního menu");
                            break;
                        case "Přepnout profil":
                            menuOptions = 2;
                            Console.WriteLine("1 - Přepnout porfil na nově vytvořený\n");
                            Console.WriteLine("2 - Zůstat na aktuálním");
                            break;
                        case "Hra probíhá | Blackjack 1. kolo":
                            menuOptions = 4;
                            Console.WriteLine("1 - Hit\n");
                            Console.WriteLine("2 - Stand\n");
                            Console.WriteLine("3 - Double\n");
                            Console.WriteLine("4 - Vzdát hru");
                            break;
                        case "Hra probíhá | Blackjack":
                            menuOptions = 2;
                            Console.WriteLine("1 - Hit\n");
                            Console.WriteLine("2 - Stand");
                            break;
                        case "Konec hry":
                            menuOptions = 2;
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
                        case "Výběr strany | Hod mincí":
                            menuOptions = 3;
                            Console.WriteLine("1 - Panna\n");
                            Console.WriteLine("2 - Orel\n");
                            Console.WriteLine("3 - Zrušit sázku");
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
                    // zde se overuje uzivateluv vstup, jestli jeho volba existuje a jestli je validni
                    // v pripade uspechu se vraci pomoci returnu
                    // jinak se while opakuje
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
                catch (Exception)
                {
                    continue;
                }
                run++;
            }

        }
    }
}
