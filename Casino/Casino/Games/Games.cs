using Casino;

namespace Casino
{
    internal class Games
    {
        public static int RandomNumber(int limit)
        {
            // 
            // funkce generujici nahodne cislo od nuly do x
            //
            // limit = do jakeho maximalniho cisla chceme generovat
            //
            Random random = new Random();
            return random.Next(limit);
        }
    }
}
