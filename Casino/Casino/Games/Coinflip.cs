using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Coinflip
    {
        public bool finished = false;
        public string winner = "";
        public int betSide;
        public int rolledSide;

        public void Game(int betSide)
        {
            this.betSide = betSide;
            this.rolledSide = Games.RandomNumber(2);
            if (betSide == this.rolledSide)
            {
                GameWinPlayer();
            }
            else
            {
                GameWinComputer();
            }
            this.finished = true;
        }
        private void GameWinPlayer()
        {
            this.winner = "player";
        }
        private void GameWinComputer()
        {
            this.winner = "computer";
        }

        private string TranslateToSide(int side)
        {
            string result = "";
            if (side == 1)
            {
                result = "Panna";
            }
            else
            {
                result = "Orel";
            }
            return result;
        }

        public void PrintResult()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("Vylosovaná strana: {0}", TranslateToSide(this.betSide)));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine(string.Format("Vaše sázka byla na stranu: {0}", TranslateToSide(this.rolledSide)));
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
