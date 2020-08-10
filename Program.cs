using System;
using Game1;
using GameCode;

/// <summary>
/// The main class.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var game = new GameMain();
        game.Test();
    }
}
