///
/// Comment out to run as Demo
///
#define Game

using DemoCode;
using Engine;


namespace GameCode
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            var engine = new EngineMain();

#if Game
            GameMain game = new GameMain(engine);
            game.Start();

#else
            Demo demo = new Demo(engine);
            demo.DemoLevel(1);
#endif

            using (engine)
                engine.Run();

        }
    }
#endif
}
