using System;

namespace WindowsGame1
{
#if WINDOWS || XBOX
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

