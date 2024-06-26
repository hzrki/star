using MetroFramework.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace msptoolui
{
    partial class home
    {
        private IContainer components = null;
        private PictureBox avatarBox; 
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "home";

            this.avatarBox = new PictureBox();
            this.avatarBox.Dock = DockStyle.Left;
            this.avatarBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.avatarBox.Size = new Size(200, 200);
            this.Controls.Add(this.avatarBox); 
        }
    }
}