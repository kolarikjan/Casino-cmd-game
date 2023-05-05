using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Casino
{
    class Program
    {
        static void Main(string[] args)
        {
            Ui environment = new Ui();
            Profiles profiles = new Profiles();
            if (!profiles.AnyProfileExists())
            {
                // pri spusteni aplikace nejdrive overime, zda existuji nejake profily, pokud ne, musi uzivatel jeden vytvorit
                int result = -1;
                while (true)
                {
                    environment.MenuHeader("Vytváření profilu");
                    if (result != 0)
                    {
                        Ui.MenuLine();
                        // nize jsou chybove hlasky pri vytvareni profilu
                        if (result == 1 || result == -1)
                        {
                            Console.WriteLine("Username může obsahovat jen písmena a čísla. Username musí být dlouhý minimálně 5 znaků.");
                        }
                        else
                        {
                            Console.WriteLine("Tento username již byl použit! Zkuste vymyslet nějaký jiný...");
                        }
                    }
                    Ui.MenuLine();
                    // profil ulozime do souboru
                    result = profiles.ProfileCreate();
                    if (result == 0)
                    {
                        // nahrajeme data do promennych pomoci funkce nize a informujeme uzivatele
                        profiles.SwitchToNewAccount();
                        Ui.MenuLine();
                        Console.Write("Profil byl úspěšně vytvořen, pokračujte stisknutím enteru...");
                        Console.ReadKey();
                        break;
                    }
                }

            }
            else
            {
                // pokud nejake profily nalezneme, dame uzivateli vybrat, na ktery se chce prihlasit
                while (true)
                {
                    environment.MenuHeader("Výběr profilu");
                    if (profiles.ProfileSelect()) break;
                }
            }
            while (true)
            {
                // hlavni while cele aplikace - nachazime se v hlavnim menu
                // vzdy uplne nahore informuje uzivatele, kde se nachazi, na jakem uctu je prihlasen a kolik penez ma
                environment.MenuHeader(string.Format("Hlavní menu | účet: {0} | ${1}", profiles.currentName, profiles.currentWallet));
                Ui.MenuLine();
                int action = environment.GenerateMenu("Hlavní menu", false);
                if (action == 1)
                {
                    // hrac zvolil, ze chce hrat hazardni hry
                    while (true)
                    {
                        if (profiles.currentWallet <= 0)
                        {
                            // zkontrolujeme, jestli ma vubec penize, pokud ne - nemuze hrat hry
                            environment.MenuHeader("Nedostatek financí");
                            Ui.MenuLine();
                            Console.Write("Na Vašem účtě se nenachází dostatek financí, pro pokračování stiskněte enter...");
                            Console.ReadKey();
                            break;
                        }
                        environment.MenuHeader(string.Format("Herní menu | účet: {0} | ${1}", profiles.currentName, profiles.currentWallet));
                        Ui.MenuLine();
                        int actionGame = environment.GenerateMenu("Herní menu", false);
                        if (actionGame == 1)
                        {
                            //
                            // Blackjack game
                            //
                            while (true)
                            {
                                while (true)
                                {
                                    environment.MenuHeader("Zadejte výšku sázky | Blackjack");
                                    Ui.MenuLine();
                                    if (profiles.AcceptBet())
                                    {
                                        break;
                                    }
                                }
                                Blackjack blackjack = new Blackjack();
                                blackjack.PrepareGame();
                                int i = 0;
                                while (blackjack.finished == false)
                                {
                                    environment.MenuHeader("Hra probíhá | Blackjack");
                                    Ui.MenuLine();
                                    blackjack.PrintCards();
                                    int actionRound;
                                    // podminka nize je kvuli tomu, ze v prvnim kole ma moznost hrat double ci hru vzdat
                                    if (i == 0)
                                    {
                                        actionRound = environment.GenerateMenu("Hra probíhá | Blackjack 1. kolo", false);
                                    }
                                    else
                                    {
                                        actionRound = environment.GenerateMenu("Hra probíhá | Blackjack", false);
                                    }
                                    if (actionRound == 3)
                                    {
                                        // pri moznosti double hrac vsadi stejnou sazku jeste jednou a jednou si lizne kartu - tim pro nej hra konci a ceka se na vysledek
                                        if (profiles.currentBet <= profiles.currentWallet)
                                        {
                                            profiles.Bet(profiles.currentBet);
                                            profiles.currentBet = profiles.currentBet * 2;
                                            blackjack.ActionRound(actionRound);
                                            break;
                                        }
                                        // muze se stat, ze vsechny posledni love hrac uz vsadit, takze uz nemuze logicky dat double, tuto informaci samozrejme dostane a bude svoje kolo opakovat
                                        Ui.MenuLine();
                                        Console.Write("Nemáte dostatek financí pro double, pro pokračování stiskněte enter...");
                                        Console.ReadKey();
                                    }
                                    else if (actionRound == 4)
                                    {
                                        // pokud se hrac vzda, prohraje pulku sazky a pulka se mu vrati
                                        blackjack.ActionRound(actionRound);
                                        profiles.currentBet = profiles.currentBet / 2;
                                        break;
                                    } 
                                    else
                                    {
                                        blackjack.ActionRound(actionRound);
                                    }
                                    i++;
                                }
                                environment.MenuHeader("Konec hry");
                                Ui.MenuLine();
                                blackjack.PrintCards(false);

                                blackjack.PostGame(profiles.currentBet);
                                profiles.CalculateWinnings(blackjack.CalculateX());

                                int gameEnd = environment.GenerateMenu("Konec hry", false);
                                if (profiles.currentWallet == 0 || gameEnd == 2)
                                {
                                    // pokud uzivatel nechce uz hrat ci nema penize, z hry ho to kopne pryc
                                    break;
                                }
                            }
                        }
                        else if (actionGame == 2)
                        {
                            //
                            // Roulette game 
                            //
                            while (true)
                            {
                                while (true)
                                {
                                    environment.MenuHeader("Zadejte výšku sázky | Ruleta");
                                    Ui.MenuLine();
                                    if (profiles.AcceptBet())
                                    {
                                        break;
                                    }
                                }
                                Roulette roulette = new Roulette();

                                int betColor = environment.GenerateMenu("Výběr barvy | Ruleta");
                                if (betColor != 4)
                                {
                                    // v tento moment si uzivatel vybral nejakou barvu, na kterou vsadil a hra muze zacit
                                    roulette.Game(betColor);

                                    environment.MenuHeader("Konec hry");
                                    Ui.MenuLine();
                                    roulette.PrintResult(profiles.currentBet);
                                    profiles.CalculateWinnings(roulette.CalculateX());

                                    int gameEnd = environment.GenerateMenu("Konec hry", false);
                                    if (profiles.currentWallet == 0 || gameEnd == 2)
                                    {
                                        // pokud uzivatel nechce uz hrat ci nema penize, z hry ho to kopne pryc
                                        break;
                                    }
                                }
                                else
                                {   // moznost 4 je vzdani se hry, jeste se nic ve hre nedelo (ani si nevybral na co vsadit), takze se mu sazka muze vratit
                                    profiles.CalculateWinnings(1);
                                    break;
                                }
                            }
                        }
                        else if (actionGame == 3)
                        {
                            //
                            // Coinflip game
                            //
                            while (true)
                            {
                                while (true)
                                {
                                    environment.MenuHeader("Zadejte výšku sázky | Hod mincí");
                                    Ui.MenuLine();
                                    if (profiles.AcceptBet())
                                    {
                                        break;
                                    }
                                }
                                Coinflip coinflip = new Coinflip();

                                int betSide = environment.GenerateMenu("Výběr strany | Hod mincí");

                                if (betSide != 3)
                                {
                                    // v tento moment si uzivatel vybral nejakou barvu, na kterou vsadil a hra muze zacit
                                    coinflip.Game(betSide);

                                    environment.MenuHeader("Konec hry");
                                    Ui.MenuLine();
                                    coinflip.PrintResult(profiles.currentBet);
                                    profiles.CalculateWinnings(coinflip.CalculateX());

                                    int gameEnd = environment.GenerateMenu("Konec hry", false);
                                    if (profiles.currentWallet == 0 || gameEnd == 2)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    // moznost 3 je vzdani se hry, jeste se nic ve hre nedelo (ani si nevybral na co vsadit), takze se mu sazka muze vratit
                                     profiles.CalculateWinnings(1);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (action == 2)
                {
                    // uzivatel se z hlavniho menu vydal do menu se spravou profilu
                    bool backToMainMenu = false;
                    while (true)
                    {
                        int actionProfiles = environment.GenerateMenu("Správa profilů");
                        if (actionProfiles == 1)
                        {
                            // vybral si, ze chce vytvorit uplne novy profil
                            int result = -1;
                            while (true)
                            {
                                environment.MenuHeader("Vytváření profilu");
                                if (result != 0)
                                {
                                    Ui.MenuLine();
                                    if (result == 1 || result == -1)
                                    {
                                        Console.WriteLine("Username může obsahovat jen písmena a čísla. Username musí být dlouhý minimálně 5 znaků.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Tento username již byl použit! Zkuste vymyslet nějaký jiný...");
                                    }
                                }
                                Ui.MenuLine();
                                // ulozeni proflu
                                result = profiles.ProfileCreate();
                                if (result == 0)
                                {
                                    int actionStay = environment.GenerateMenu("Přepnout profil");
                                    // uzivateli dame na vyber, zda se chce prepnout do noveho profilu ci jestli chce zustat na profilu aktualnim
                                    if (actionStay == 1)
                                    {
                                        profiles.SwitchToNewAccount();
                                        backToMainMenu = true;
                                    }
                                    break;
                                }
                            }
                        }
                        else if (actionProfiles == 2)
                        {
                            // vybral si, ze se chce odhlasit a prepnout se na jiny profil
                            while (true)
                            {
                                environment.MenuHeader("Výběr profilu");
                                backToMainMenu = true;
                                if (profiles.ProfileSelect()) break;
                            }
                        }
                        else if (actionProfiles == 3)
                        {
                            // vybral si smazani vsech souboru
                            profiles.DeleteAccounts();

                            // pote logicky si musi hned vytvorit novy profil, aby aplikace mohla fungovat normalne
                            if (!profiles.AnyProfileExists())
                            {
                                int result = -1;
                                while (true)
                                {
                                    environment.MenuHeader("Vytváření profilu");
                                    if (result != 0)
                                    {
                                        Ui.MenuLine();
                                        if (result == 1 || result == -1)
                                        {
                                            Console.WriteLine("Username může obsahovat jen písmena a čísla. Username musí být dlouhý minimálně 5 znaků.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Tento username již byl použit! Zkuste vymyslet nějaký jiný...");
                                        }
                                    }
                                    Ui.MenuLine();
                                    result = profiles.ProfileCreate();
                                    if (result == 0)
                                    {
                                        backToMainMenu = true;
                                        profiles.SwitchToNewAccount();
                                        Ui.MenuLine();
                                        Console.Write("Profil byl úspěšně vytvořen, pokračujte stisknutím enteru...");
                                        Console.ReadKey();
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // vraceni se do hlavniho menu
                            break;
                        }
                        if (backToMainMenu)
                        {
                            // v pripade, ze chceme dvojity break
                            break;
                        }
                    }
                    
                }
                else
                {
                    // vypnuti aplikace
                    Environment.Exit(0);
                }
            }
        }
    
    }
}