using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    internal class Coinflip
    {
        //
        // trida pro hru coinflip
        //

        public bool finished = false;
        public string winner = "";
        public int betSide;
        public int rolledSide;

        public void Game(int betSide)
        {
            //
            // metoda, ktera obstarava samotnou hru - ulozi na co hrac vsadil, vylosuje random cislo a rozhoduje o tom, kdo vyhral
            //
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
            //
            // uklada informaci o tom, ze hrac vyhral
            //
            this.winner = "player";
        }
        private void GameWinComputer()
        {
            //
            // uklada informaci o tom, ze pocitac vyhral
            //
            this.winner = "computer";
        }

        private string TranslateToSide(int side)
        {
            //
            // preklada cislo na stranu mince (1-panna, 2-orel)
            //
            // side = zde vkladame informaci o tom, jakou stranu mince chceme zjistit
            //
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
        public int CalculateX()
        {
            //
            // pocita jakym cislem nasobime vyhru
            //
            int result = 0;
            if (this.winner == "player")
            {
                result = 2;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public void PrintResult(int currentBet = 0)
        {
            //
            // metoda, vypisujici infomraci o tom kdo vyhral, pripadne kolik kdo vyhral a prohral
            //
            // currentBet = jaka byla vsazena castka
            //
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
                    Console.WriteLine(string.Format("Vaše výhra činní ${0}", currentBet * 2));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
                case "computer":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bohužel jste prohrál/a!\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("Celkem jste ztratil/a ${0}", currentBet));
                    Console.ResetColor();
                    Ui.MenuLine();
                    break;
            }
        }
    }
}
