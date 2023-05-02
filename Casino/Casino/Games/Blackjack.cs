using Casino;

namespace Casino
{
    internal class Blackjack
    {
        public string winner = "";
        public bool finished = false;

        private List<string> playerCards = new List<string>();
        private List<string> croupierCards = new List<string>();

        private int playerScore;
        private int croupierScore;

        private List<string> deck = new List<string>();

        public void PrepareGame()
        {
            GenerateDeck();

            for (int i = 0; i < 4; i++)
            {
                int randomNumber = Games.RandomNumber(this.deck.Count);
                if (i % 2 == 1)
                {
                    this.playerCards.Add(this.deck[randomNumber]);
                }
                else
                {
                    this.croupierCards.Add(this.deck[randomNumber]);
                }
                this.deck.RemoveAt(randomNumber);
            }

            this.playerScore = CountHand(this.playerCards);
            this.croupierScore = CountHand(this.croupierCards);

            Check21();
        }
        public void PrintCards(bool hideCroupierCards = true)
        {
            Console.Write("Vaše karty: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (string card in this.playerCards)
            {
                Console.Write(card + " ");
            }
            Console.ResetColor();
            Console.Write(string.Format("({0})\n\n", this.playerScore));

            Console.Write("Krupiérovy karty: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < this.croupierCards.Count; i++)
            {
                if (hideCroupierCards && i != 0)
                {
                    Console.Write("XX ");
                }
                else
                {
                    Console.Write(this.croupierCards[i] + " ");
                };
            }
            Console.ResetColor();
            if (hideCroupierCards)
            {
                Console.Write("(??)\n");
            }
            else
            {
                Console.Write(string.Format("({0})\n", this.croupierScore));
            }
            Ui.MenuLine();
        }
        private void Check21()
        {
            int player = this.playerScore;
            int croupier = this.croupierScore;

            int[] result = new int[2];

            if (croupier == 21 && player < 21)
            {
                GameWinCroupier();
            }
            else if (player == 21 && croupier < 21)
            {
                GameWinPlayer();
            }
            else if (player == 21 && croupier == 21)
            {
                GameTied();
            }
        }
        private void GameWinPlayer()
        {
            this.winner = "player";
            this.finished = true;
        }
        private void GameWinCroupier()
        {
            this.winner = "croupier";
            this.finished = true;
        }
        private void GameTied()
        {
            this.winner = "tied";
            this.finished = true;
        }
        private void CheckScore()
        {
            if (this.playerScore > 21)
            {
                GameWinCroupier();
            }
            else if (this.croupierScore > 21)
            {
                GameWinPlayer();
            }
            else if (this.croupierScore > 16)
            {
                if (this.playerScore == this.croupierScore)
                {
                    GameTied();
                }
                else if (this.playerScore > this.croupierScore)
                {
                    GameWinPlayer();
                }
                else
                {
                    GameWinCroupier();
                }

            }
        }
        public void ActionRound(int action)
        {
            switch (action)
            {
                case 1:
                    SelectRandomCard("player");
                    CheckScore();
                    break;
                case 2:
                    while (this.croupierScore <= 16)
                    {
                        SelectRandomCard("croupier");
                    }
                    CheckScore();
                    break;
                case 3:
                    SelectRandomCard("player");
                    while (this.croupierScore <= 16)
                    {
                        SelectRandomCard("croupier");
                    }
                    CheckScore();
                    break;
                case 4:
                    this.finished = true;
                    this.winner = "none";
                    break;
            }
        }
        public void PostGame()
        {
            switch (winner)
            {
                case "player":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Vyhrál/a jste, gratulujeme!\n");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(string.Format("Vaše výhra činní ${0}", "500"));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
                case "croupier":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bohužel jste prohrál/a!\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("Celkem jste ztratil/a ${0}", "500"));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
                case "tied":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Hra skončila remízou!\n");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(string.Format("Vaše sázka v hodnotě ${0} se Vám vrátila zpět do peněženky", "500"));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
            }
        }
        private void SelectRandomCard(string who)
        {
            int randomNumber = Games.RandomNumber(this.deck.Count);
            if (who == "player")
            {
                this.playerCards.Add(this.deck[randomNumber]);
                this.playerScore = CountHand(this.playerCards);
            }
            else
            {
                this.croupierCards.Add(this.deck[randomNumber]);
                this.croupierScore = CountHand(this.croupierCards);
            }
            this.deck.RemoveAt(randomNumber);
            Check21();
        }

        private int CountHand(List<string> hand)
        {
            int total = 0;
            int aceCount = 0;
            foreach (string handCard in hand)
            {
                string cardType = handCard.Length == 3 ? handCard[1].ToString() + handCard[2].ToString() : handCard[1].ToString();
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
        private void GenerateDeck()
        {
            string[] cards = { "Q", "J", "K", "A", "2", "4", "5", "6", "7", "8", "9", "10" };
            string[] types = { "♦", "♣", "♠", "♥" };
            foreach (string card in cards)
            {
                foreach (string type in types)
                {
                    deck.Add(type + card);
                }
            }
        }
    }
}
