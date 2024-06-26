namespace msptoolui
{
    partial class login
    {
        private System.ComponentModel.IContainer components = null;
        private MetroFramework.Controls.MetroTextBox usernameBox;
        private MetroFramework.Controls.MetroTextBox pwBox;
        private MetroFramework.Controls.MetroComboBox serverCBox;
        private MetroFramework.Controls.MetroButton loginButton;
        private MetroFramework.Controls.MetroLabel loginSlabel;

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
            this.usernameBox = new MetroFramework.Controls.MetroTextBox();
            this.pwBox = new MetroFramework.Controls.MetroTextBox();
            this.serverCBox = new MetroFramework.Controls.MetroComboBox();
            this.loginButton = new MetroFramework.Controls.MetroButton();
            this.loginSlabel = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();

            this.usernameBox.Location = new System.Drawing.Point(23, 63);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(200, 23);
            this.usernameBox.TabIndex = 0;
            this.usernameBox.PromptText = "Enter username";

            this.pwBox.Location = new System.Drawing.Point(23, 92);
            this.pwBox.Name = "pwBox";
            this.pwBox.PasswordChar = '*';
            this.pwBox.Size = new System.Drawing.Size(200, 23);
            this.pwBox.TabIndex = 1;
            this.pwBox.PromptText = "Enter password";

            
            
            var choices = new[]
            {
                "GB", "US", "TR", "SE", "FR", "DE", "NL", "FI", "NO", "DK", "CA", "AU", "PL", "NZ", "IE", "ES"
            };
            this.serverCBox.FormattingEnabled = true;
            this.serverCBox.Items.AddRange(choices);
            this.serverCBox.ItemHeight = 23;
            this.serverCBox.Location = new System.Drawing.Point(23, 121);
            this.serverCBox.Name = "serverCBox";
            this.serverCBox.Size = new System.Drawing.Size(200, 29);
            this.serverCBox.TabIndex = 2;

            this.loginButton.Location = new System.Drawing.Point(23, 156);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(200, 23);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Login";
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);

            this.loginSlabel.AutoSize = true;
            this.loginSlabel.Location = new System.Drawing.Point(23, 182);
            this.loginSlabel.Name = "loginSlabel";
            this.loginSlabel.Size = new System.Drawing.Size(75, 19);
            this.loginSlabel.TabIndex = 4;
            this.loginSlabel.Text = "Logging in...";
            this.loginSlabel.Visible = false;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 220);
            this.Controls.Add(this.loginSlabel);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.serverCBox);
            this.Controls.Add(this.pwBox);
            this.Controls.Add(this.usernameBox);
            this.Name = "Login";
            this.Text = "MSPTool v1.6";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
