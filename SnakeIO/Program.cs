using System;

namespace Yew 
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SnakeIO.SnakeIO())
            {
                game.Run();
            }
        }
    }
}
