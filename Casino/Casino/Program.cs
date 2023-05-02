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
            if (profiles.ProfileExist())
            {
                environment.MenuHeader("Vytváření profilu");
                profiles.ProfileCreate();
            }
            else
            {
                while (true)
                {
                    int action = environment.GenerateMenu("Hlavní menu");
                    if (action == 1)
                    {
                        while (true)
                        {
                            int actionGame = environment.GenerateMenu("Herní menu");
                            if (actionGame == 1)
                            {
                                //
                                // Blackjack game
                                //
                                while (true)
                                {
                                    Blackjack blackjack = new Blackjack();
                                    blackjack.PrepareGame();

                                    while (blackjack.finished == false)
                                    {
                                        environment.MenuHeader("Hra probíhá | Blackjack");
                                        Ui.MenuLine();
                                        blackjack.PrintCards();

                                        int actionRound = environment.GenerateMenu("Hra probíhá | Blackjack", false);
                                        blackjack.ActionRound(actionRound);

                                        if (actionRound == 3 || actionRound == 4)
                                        {
                                            break;
                                        }
                                    }
                                    environment.MenuHeader("Konec hry");
                                    Ui.MenuLine();
                                    blackjack.PrintCards(false);
                                    blackjack.PostGame();
                                    int gameEnd = environment.GenerateMenu("Konec hry", false);

                                    if (gameEnd == 2)
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
                                    Roulette roulette = new Roulette();

                                    int betColor = environment.GenerateMenu("Výběr barvy | Ruleta");
                                    if (betColor != 4)
                                    {
                                        roulette.Game(betColor);

                                        environment.MenuHeader("Konec hry");
                                        Ui.MenuLine();
                                        roulette.PrintResult();
                                        int gameEnd = environment.GenerateMenu("Konec hry", false);
                                    }
                                    else
                                    {
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
                                    Coinflip coinflip = new Coinflip();

                                    int betSide = environment.GenerateMenu("Výběr strany | Hod mincí");

                                    if (betSide != 3)
                                    {
                                        coinflip.Game(betSide);

                                        environment.MenuHeader("Konec hry");
                                        Ui.MenuLine();
                                        coinflip.PrintResult();
                                        int gameEnd = environment.GenerateMenu("Konec hry", false);
                                    }
                                    else
                                    {
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
                        action = environment.GenerateMenu();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
    
    }
}