using Casino;

namespace Casino
{
    internal class Roulette
    {
        public int rolledNumber;
        public string rolledColor = "";
        public int betColor;
        public string betColorName = "";
        public string winner = "";

        public void Game(int betColor)
        {
            this.betColor = betColor;
            this.TranslateBetToColor();
            this.RollNumber();
            switch (betColor)
            {
                case 1:
                    if (this.rolledNumber != 0 && this.rolledNumber % 2 == 0)
                    {
                        this.GameWinPlayer();
                    } 
                    else
                    {
                        this.GameWinComputer();
                    }
                    break;
                case 2:
                    if (this.rolledNumber != 0 && this.rolledNumber % 2 == 1)
                    {
                        this.GameWinPlayer();
                    }
                    else
                    {
                        this.GameWinComputer();
                    }
                    break;
                case 3:
                    if (this.rolledNumber == 0)
                    {
                        this.GameWinPlayer();
                    }
                    else
                    {
                        this.GameWinComputer();
                    }
                    break;
            }
        }
        public int CalculateX()
        {
            int result = 0;
            if (this.winner == "player")
            {
                if (betColor == 0)
                {
                    result = 14;
                }
                else
                {
                    result = 2;
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        private void GameWinPlayer()
        {
            this.winner = "player";
        }
        private void GameWinComputer()
        {
            this.winner = "computer";
        }
        private void RollNumber()
        {
            int num = 0;
            for (int i = 0; i < Games.RandomNumber(10); i++)
            {
                num = Games.RandomNumber(37);
            }
            this.rolledNumber = num;
            if (this.rolledNumber == 0)
            {
                this.rolledColor = "zelená";
            }
            else if (this.rolledNumber % 2 == 0)
            {
                this.rolledColor = "červená";
            }
            else
            {
                this.rolledColor = "černá";
            }
        }
        private void TranslateBetToColor()
        {
            if (this.betColor == 1)
            {
                this.betColorName = "červená";
            }
            else if (this.betColor == 2)
            {
                this.betColorName = "černá";
            }
            else
            {
                this.betColorName = "zelená";
            }
        }
        public void PrintResult()
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("Vylosované číslo: {0}", this.rolledNumber));
            Console.WriteLine();
            Console.WriteLine(string.Format("Vylosované barva: {0}", this.rolledColor));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine(string.Format("Vaše sázka byla na barvu: {0}", this.betColorName));
            Console.ResetColor();
            Ui.MenuLine();

            switch (this.winner)
            {
                case "player":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Vyhrál/a jste, gratulujeme!\n");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(string.Format("Vaše výhra činní ${0}", "500"));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
                case "computer":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bohužel jste prohrál/a!\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("Celkem jste ztratil/a ${0}", "500"));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
            }
        }
    }
}
