using Xenko.Engine;

namespace DemoArmyBuilder.Windows
{
    class DemoArmyBuilderApp
    {
        static void Main(string[] args)
        {
            using (var game = new DemoArmyBuilderGame())
            {
                game.Run();
            }
        }
    }
}
