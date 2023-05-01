using Casino;

namespace Casino
{
    internal class Games
    {
        public static int RandomNumber(int limit)
        {
            Random random = new Random();
            return random.Next(limit);
        }
    }
}
