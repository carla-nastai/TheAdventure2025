namespace TheAdventure
{
    partial class MainMenu
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.btnEasyMode = new System.Windows.Forms.Button();
            this.btnHardMode = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(188, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "The Adventure";

            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(12, 60);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(114, 45);
            this.btnStartGame.TabIndex = 1;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = false;
            this.btnStartGame.BackColor = System.Drawing.Color.Green;
            this.btnStartGame.ForeColor = System.Drawing.Color.Black; // Text color
            this.btnStartGame.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);

            // 
            // btnEasyMode
            // 
            this.btnEasyMode.Location = new System.Drawing.Point(12, 111);
            this.btnEasyMode.Name = "btnEasyMode";
            this.btnEasyMode.Size = new System.Drawing.Size(114, 45);
            this.btnEasyMode.TabIndex = 2;
            this.btnEasyMode.Text = "Easy Mode";
            this.btnEasyMode.UseVisualStyleBackColor = false;
            this.btnEasyMode.BackColor = System.Drawing.Color.Brown;
            this.btnEasyMode.ForeColor = System.Drawing.Color.Black; // Text color
            this.btnEasyMode.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEasyMode.Click += new System.EventHandler(this.btnEasyMode_Click);

            // 
            // btnHardMode
            // 
            this.btnHardMode.Location = new System.Drawing.Point(12, 162);
            this.btnHardMode.Name = "btnHardMode";
            this.btnHardMode.Size = new System.Drawing.Size(114, 45);
            this.btnHardMode.TabIndex = 3;
            this.btnHardMode.Text = "Hard Mode";
            this.btnHardMode.UseVisualStyleBackColor = false;
            this.btnHardMode.BackColor = System.Drawing.Color.Red;
            this.btnHardMode.ForeColor = System.Drawing.Color.Black; // Text color
            this.btnHardMode.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHardMode.Click += new System.EventHandler(this.btnHardMode_Click);

            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(12, 213);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(114, 45);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.BackColor = System.Drawing.Color.Gray;
            this.btnQuit.ForeColor = System.Drawing.Color.Black; // Text color
            this.btnQuit.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);

            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400); // Increased size
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnHardMode);
            this.Controls.Add(this.btnEasyMode);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.lblTitle);
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.BackColor = System.Drawing.Color.DarkBlue;

            try
            {
                // Set the background image
                this.BackgroundImage = Image.FromFile("./Assets/MainMenuBackground.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image: {ex.Message}");
            }

            // Center the controls
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            this.lblTitle.Location = new System.Drawing.Point(centerX - this.lblTitle.Width / 2, 20);
            this.btnStartGame.Location = new System.Drawing.Point(centerX - this.btnStartGame.Width / 2, 100);
            this.btnEasyMode.Location = new System.Drawing.Point(centerX - this.btnEasyMode.Width / 2, 160);
            this.btnHardMode.Location = new System.Drawing.Point(centerX - this.btnHardMode.Width / 2, 220);
            this.btnQuit.Location = new System.Drawing.Point(centerX - this.btnQuit.Width / 2, 280);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Button btnEasyMode;
        private System.Windows.Forms.Button btnHardMode;
        private System.Windows.Forms.Button btnQuit;
    }
}