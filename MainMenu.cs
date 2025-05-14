using System;
using System.Windows.Forms;
using Silk.NET.SDL;
namespace TheAdventure
{
    public partial class MainMenu : Form
    {
        private readonly Sdl _sdl;

        public MainMenu(Sdl sdl)
        {
            InitializeComponent();
            _sdl = sdl;

            try
            {
                this.BackgroundImage = System.Drawing.Image.FromFile("./Assets/MainMenuBackground.png");  // Adjust the path if needed
                this.BackgroundImageLayout = ImageLayout.Stretch;  // This will stretch the image to fill the form
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image: {ex.Message}");
            }

            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            StartGame("Normal");
        }

        private void btnEasyMode_Click(object sender, EventArgs e)
        {
            StartGame("Easy");
        }

        private void btnHardMode_Click(object sender, EventArgs e)
        {
            StartGame("Hard");
        }

      

        private void StartGame(string difficulty)
        {
            // Hide the main menu
            this.Hide();

            // Create the game window, renderer, and engine
            var gameWindow = new GameWindow(_sdl);
            var input = new Input(_sdl);
            var gameRenderer = new GameRenderer(_sdl, gameWindow);
            var engine = new Engine(gameRenderer, input,gameWindow);

            // Start the game with the selected difficulty
            engine.StartGame(difficulty);

            // Main game loop
            bool quit = false;
            while (!quit)
            {
                quit = input.ProcessInput();
                if (quit) break;

                engine.ProcessFrame();
                engine.RenderFrame();

                System.Threading.Thread.Sleep(13); // Control frame rate
            }

            // Close the application after the game loop ends
            Application.Exit();
        }
    }
}