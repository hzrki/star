using MetroFramework.Forms;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace msptoolui
{
    public partial class home : MetroForm
    {

        public home(string server, string ticket, int actorId, string accessToken, string profileId, string loginId)
        {
            InitializeComponent();


            string avatarjpg = $"https://snapshots.mspcdns.com/v1/MSP/{server}/snapshot/fullSizeMoviestar/{actorId}.jpg";

            using (WebClient msptoolui = new WebClient())
            {
                byte[] loc1 = msptoolui.DownloadData(avatarjpg);

                using (MemoryStream loc2 = new MemoryStream(loc1))
                {
                    Image avatar = Image.FromStream(loc2);

                    avatarBox.Image = avatar;
                    avatarBox.SizeMode = PictureBoxSizeMode.Zoom;
                    avatarBox.Size = new Size(200, 200); 
                }
            }
        }
    }
}
