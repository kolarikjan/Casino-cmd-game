using System;
using System.IO;
using System.Collections.Generic;

namespace Casino
{
    class Program
    {
        static void MenuHeader(string title = "")
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write("Casino");
            if (title != "")
            {
                Console.Write(" - " + title);
            }
            Console.WriteLine();
        }
        static void MenuLine()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------");
            Console.WriteLine();
        }
        static int GenerateMenu(string title = "")
        {
            while (true)
            {
                MenuHeader(title);
                MenuLine();
                int menuOptions = 0;
                switch (title)
                {
                    case "hlavní menu":
                        menuOptions = 3;
                        Console.WriteLine("1 - Hrát hazardní hry");
                        Console.WriteLine("2 - Správa profilů");
                        Console.WriteLine("3 - Ukončit aplikaci");
                        break;
                    case "herní menu":
                        menuOptions = 3;
                        Console.WriteLine("1 - Blackjack");
                        Console.WriteLine("2 - Ruleta");
                        Console.WriteLine("3 - Hod mincí");
                        Console.WriteLine("4 - Ukončit aplikaci");
                        break;
                    default:
                        Console.WriteLine("Zatím tady nic není");
                        break;
                }
                MenuLine();
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
            }
            
        }
        static void Blackjack()
        {
            List<string> deck = GenerateDeck();
            List<string> playerCards = new List<string>();
            List<string> croupierCards = new List<string>();

            for (int i = 1; i <= 4; i++)
            {
                Random random = new Random();
                int randomNumber = random.Next(deck.Count);
                if (randomNumber % 2 == 1)
                {
                    playerCards.Add(deck[randomNumber]);
                }
                else
                {
                    croupierCards.Add(deck[randomNumber]);
                }
                deck.RemoveAt(randomNumber);
            }

            int playerScore = countHand(playerCards);
            int croupierScore = countHand(croupierCards);

            Console.WriteLine(playerScore+ " " + croupierScore);
        }
        static int countHand(List<string> hand)
        {
            int total = 0;
            int aceCount = 0;
            foreach (string handCard in hand)
            {
                string cardType = handCard[1].ToString();
                if (cardType == "A")
                {
                    aceCount++;
                }
                else
                {
                    if (cardType == "K" || cardType == "J" || cardType == "Q")
                    {
                        total = total + 10;
                    } 
                    else
                    {
                        total = total + int.Parse(cardType);
                    }
                }
            }
            if (aceCount > 0)
            {
                for (int i = 0; i < aceCount; i++)
                {
                    if (total + 11 <= 21)
                    {
                        total = total + 11;
                    }
                    else
                    {
                        total = total + 1;
                    }
                }
            }
            return total;
        }
        static List<string> GenerateDeck()
        {

            string[] cards = { "Q", "J", "K", "A", "2", "4", "5", "6", "7", "8", "9", "10" };
            string[] types = { "♦", "♣", "♠", "♥" };
            List<string> deck = new List<string>(); ;
            foreach (string card in cards)
            {
                foreach (string type in types)
                {
                    deck.Add(type + card);
                }
            }
            return deck;
        }
        static void Roulette ()
        {

        }
        static void Main(string[] args)
        {
            while (true)
            {
                int action = GenerateMenu("hlavní menu");
                if (action == 1)
                {
                    int actionGame = GenerateMenu("herní menu");
                    if (actionGame == 1)
                    {
                        Blackjack();
                    }
                }
                else if (action == 2)
                {
                    action = GenerateMenu();
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