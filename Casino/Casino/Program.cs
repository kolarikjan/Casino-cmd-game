using System;
using System.IO;
using System.Collections.Generic;

namespace Casino
{
    class Program
    {
        static void Main(string[] args)
        {
            Ui environment = new Ui();
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
                                    roulette.ColorSelected(betColor);
                                }
                            }
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
                Console.ReadLine();
            }
            
        }
    
    }
}