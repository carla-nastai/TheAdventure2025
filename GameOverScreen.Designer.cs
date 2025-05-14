namespace TheAdventure
{
    partial class GameOverScreen
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnMainMenu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnMainMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // btnMainMenu
            this.btnMainMenu.Name = "btnMainMenu";
            this.btnMainMenu.Size = new System.Drawing.Size(114, 45);
            this.btnMainMenu.TabIndex = 1;
            this.btnMainMenu.Text = "Main Menu";
            this.btnMainMenu.UseVisualStyleBackColor = false;
            this.btnMainMenu.BackColor = System.Drawing.Color.Pink;
            this.btnMainMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMainMenu.Click += new System.EventHandler(this.btnMainMenu_Click);

            // GameOverScreen Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 500); // Adjusted size to be more portrait-oriented
            this.Controls.Add(this.btnMainMenu);
            this.Name = "GameOverScreen";
            this.Text = "Game Over";
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; // Center the form on the screen

            // Position the button at the bottom center
            this.btnMainMenu.Location = new System.Drawing.Point((this.ClientSize.Width - this.btnMainMenu.Width) / 2, this.ClientSize.Height - 60);

            this.ResumeLayout(false);
        }

       
    }
}