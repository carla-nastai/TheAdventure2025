using System;
using System.Windows.Forms;
using Silk.NET.SDL;

namespace TheAdventure
{
    public partial class GameOverScreen : Form
    {
        private readonly Engine _engine;

        public GameOverScreen(Engine engine)
        {
            InitializeComponent();
            _engine = engine;
             try
    {
        this.BackgroundImage = System.Drawing.Image.FromFile("./Assets/GameOverBackground.png");  // Adjust the path if needed
        this.BackgroundImageLayout = ImageLayout.Stretch;  // This will stretch the image to fill the form
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading background image: {ex.Message}");
    }
        }

private void btnMainMenu_Click(object sender, EventArgs e)
{
    // Hide the game over screen
    this.Hide();

    // Restart the application or show the main menu
    Application.Restart();
}
    }
}