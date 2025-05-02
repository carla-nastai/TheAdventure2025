using Silk.NET.SDL;

namespace TheAdventure
{
    public unsafe class GameWindow
    {
        private static GameWindow? _instance;
        private static readonly object _instanceLock = new object();
        private readonly IntPtr _window;
        private readonly Sdl _sdl;

        public GameWindow(Sdl sdl)
        {
            _sdl = sdl;
            _window = (IntPtr)sdl.CreateWindow(
                "The Adventure", Sdl.WindowposUndefined, Sdl.WindowposUndefined, 800, 800,
                (uint)WindowFlags.Resizable | (uint)WindowFlags.AllowHighdpi
            );

            if (_window == IntPtr.Zero)
            {
                var ex = sdl.GetErrorAsException();
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("Failed to create window.");
            }
        }

        public static GameWindow GetInstance(Sdl sdl)
{
    lock (_instanceLock)
    {
        if (_instance == null)
        {
            Console.WriteLine("Creating new instance of GameWindow");
            _instance = new GameWindow(sdl);
        }
    }

    return _instance;
}

        public IntPtr CreateRenderer()
        {
            var renderer = (IntPtr)_sdl.CreateRenderer((Window*)_window, -1, (uint)RendererFlags.Accelerated);
            if (renderer == IntPtr.Zero)
            {
                var ex = _sdl.GetErrorAsException();
                if (ex != null)
                {
                    throw ex;
                }

                throw new Exception("Failed to create renderer.");
            }

            return renderer;
        }

        public void Destroy()
        {
            _sdl.DestroyWindow((Window*)_window);
        }
    }
}