using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using static msptoolui.Checksum;

namespace msptoolui
{
    public partial class login : MetroForm
    {
        public login()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;
            string password = pwBox.Text;
            string server = serverCBox.SelectedItem?.ToString();


            loginSlabel.Text = "Logging in...";
            loginSlabel.Visible = true;

            dynamic login = null;

            await Task.Run(() =>
            {
                login = AMFConn(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                    new object[6]
                    {
                        username, password, new object[] { }, null, null, "MSP1-Standalone:XXXXXX"
                    });
                Thread.Sleep(1000);
            });

            if (login["loginStatus"]["status"] != "Success")
            {
                MessageBox.Show("Login failed");
                loginSlabel.Visible = false;
            }
            else
            {
                MessageBox.Show("Login successful!");
                loginSlabel.Visible = false;
                int actorId = login["loginStatus"]["actor"]["ActorId"];
                string name = login["loginStatus"]["actor"]["Name"];
                string ticket = login["loginStatus"]["ticket"];
                string accessToken = login["loginStatus"]["nebulaLoginStatus"]["accessToken"];
                string profileId = login["loginStatus"]["nebulaLoginStatus"]["profileId"];
                var th = new JwtSecurityTokenHandler();
                var jtoken = th.ReadJwtToken(accessToken);
                var loginId = jtoken.Payload["loginId"].ToString();
                home home = new home(server, name, ticket, actorId, accessToken, profileId, loginId);
                home.Show();
                Hide();
            }
        }
    }
}