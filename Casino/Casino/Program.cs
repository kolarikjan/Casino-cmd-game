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
                while (true)
                {
                    environment.MenuHeader("Výběr profilu");
                    if (profiles.ProfileSelect()) break;
                }
            }
            while (true)
            {
                environment.MenuHeader(string.Format("Hlavní menu | účet: {0} | ${1}", profiles.currentName, profiles.currentWallet));
                Ui.MenuLine();
                int action = environment.GenerateMenu("Hlavní menu", false);
                if (action == 1)
                {
                    while (true)
                    {
                        if (profiles.currentWallet <= 0)
                        {
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

                                while (blackjack.finished == false)
                                {
                                    environment.MenuHeader("Hra probíhá | Blackjack");
                                    Ui.MenuLine();
                                    blackjack.PrintCards();

                                    int actionRound = environment.GenerateMenu("Hra probíhá | Blackjack", false);
                                    if (actionRound == 3)
                                    {
                                        if (profiles.currentBet <= profiles.currentWallet)
                                        {
                                            profiles.Bet(profiles.currentBet);
                                            profiles.currentBet = profiles.currentBet * 2;
                                            blackjack.ActionRound(actionRound);
                                            break;
                                        }
                                        Ui.MenuLine();
                                        Console.Write("Nemáte dostatek financí pro double, pro pokračování stiskněte enter...");
                                        Console.ReadKey();
                                    }
                                    else if (actionRound == 4)
                                    {
                                        blackjack.ActionRound(actionRound);
                                        profiles.currentBet = profiles.currentBet / 2;
                                        break;
                                    } 
                                    else
                                    {
                                        blackjack.ActionRound(actionRound);
                                    }
                                }
                                environment.MenuHeader("Konec hry");
                                Ui.MenuLine();
                                blackjack.PrintCards(false);
                                blackjack.PostGame(profiles.currentBet);
                                profiles.CalculateWinnings(blackjack.CalculateX());
                                int gameEnd = environment.GenerateMenu("Konec hry", false);
                                if (profiles.currentWallet == 0 || gameEnd == 2)
                                {
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
                                    roulette.Game(betColor);

                                    environment.MenuHeader("Konec hry");
                                    Ui.MenuLine();
                                    roulette.PrintResult(profiles.currentBet);
                                    profiles.CalculateWinnings(roulette.CalculateX());
                                    int gameEnd = environment.GenerateMenu("Konec hry", false);
                                    if (profiles.currentWallet == 0 || gameEnd == 2)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
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
                    bool backToMainMenu = false;
                    while (true)
                    {
                        int actionProfiles = environment.GenerateMenu("Správa profilů");
                        if (actionProfiles == 1)
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
                                    int actionStay = environment.GenerateMenu("Přepnout profil");
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
                            while (true)
                            {
                                environment.MenuHeader("Výběr profilu");
                                backToMainMenu = true;
                                if (profiles.ProfileSelect()) break;
                            }
                        }
                        else if (actionProfiles == 3)
                        {
                            profiles.DeleteAccounts();
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
                            else
                            {
                                environment.MenuHeader("Výběr profilu");
                                if (profiles.ProfileSelect()) break;
                            }
                        }
                        else
                        {
                            break;
                        }
                        if (backToMainMenu)
                        {
                            break;
                        }
                    }
                    
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    
    }
}