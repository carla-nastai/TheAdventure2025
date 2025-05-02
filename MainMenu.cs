using System;
using System.Windows.Forms;
using Silk.NET.SDL;

namespace TheAdventure
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

      private void btnStartGame_Click(object sender, EventArgs e)
{
    // Hide the main menu
    this.Hide();

    // Create an instance of GameLogic
    var gameLogic = new GameLogic();

    // Start the game
    gameLogic.StartGame();
}
        private void btnEasyMode_Click(object sender, EventArgs e)
        {
            // Logic to start the game in easy mode
            MessageBox.Show("Starting the game in Easy Mode...");
            // You can add code here to start your game logic with easy mode settings
        }

        private void btnHardMode_Click(object sender, EventArgs e)
        {
            // Logic to start the game in hard mode
            MessageBox.Show("Starting the game in Hard Mode...");
            // You can add code here to start your game logic with hard mode settings
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // Logic to quit the game
            Application.Exit();
        }
    }
}