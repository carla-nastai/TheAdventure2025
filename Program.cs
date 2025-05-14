using System;
using System.Threading;
using Silk.NET.SDL;

namespace TheAdventure
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            // Initialize SDL
            var sdl = new Sdl(new SdlContext());
            var sdlInitResult = sdl.Init(Sdl.InitVideo | Sdl.InitAudio | Sdl.InitEvents | Sdl.InitTimer |
                                         Sdl.InitGamecontroller |
                                         Sdl.InitJoystick);

            if (sdlInitResult < 0)
            {
                throw new InvalidOperationException("Failed to initialize SDL.");
            }

            // Create and show the main menu
            var mainMenu = new MainMenu(sdl);
            mainMenu.ShowDialog(); // Show main menu and wait for user to select difficulty

            // Quit SDL once the main menu is closed
            sdl.Quit();
        }
    }
}