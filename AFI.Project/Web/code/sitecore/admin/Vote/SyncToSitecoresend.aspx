<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncToSitecoresend.aspx.cs" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>AFI Vote</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
  </head>
  <body class="configure-page">
    <form id="form1" runat="server">
      <header>
        <div class="icon-container">
          <a href="/sitecore/shell/sitecore/client/applications/launchpad" title="Back to Lunchpad" class="back"
            ><img src="./image/back.svg" alt="back"
          /></a>
          <a href="/sitecore/admin/vote/configure-vote.aspx?sc_bw=1" title="Vote configure" class="setting">
            <img src="./image/gear.png" alt="configure" style="width: 40px" />
          </a>
        </div>

        <div class="user-info">
          <asp:Button
            ID="btnLogout"
            usesubmitbehavior="false"
            type="button"
            runat="server"
            Text="Logout"
            OnClick="btnLogout_Click"
            class="logout-btn"
          />
          <asp:Label ID="lblUsername" runat="server" Text="" class="username"></asp:Label>

          <img src="./image/avatar.png" alt="User Photo" class="user-photo" />
        </div>
      </header>
      <div class="logo" style="margin: 50px auto;width: 200px;">
        <img src="./image/logo.png" alt="Logo" />
      </div>
      <div class="box configure-box">
    

        <h1>Work In Progress</h1>

     
      </div>

      <!-- Popup -->
      <div class="popup" id="popup">
        <h2>Are you sure you want to change configure?</h2>
        <div class="button__container">
          <button type="button" class="btn" id="configure-no-button">No</button>
          <button type="button" class="btn" id="configure-yes-button">Yes</button>
        </div>
      </div>

      <script>
        window.addEventListener("DOMContentLoaded", () => {
          const configureBtn = document.getElementById("configureBtn");
          const popup = document.getElementById("popup");
          const YesButton = document.getElementById("configure-yes-button");
          const NoButton = document.getElementById("configure-no-button");

          // Function to show the popup
          configureBtn.addEventListener("click", function () {
            popup.classList.add("show");
          });

          // Function to handle the user's response
          function confirmConfigure(confirm) {
            if (confirm) {
              var currentHost = window.location.protocol + "//" + window.location.host;
              window.location.href = currentHost + "/sitecore/admin/vote/configure-vote.aspx?sc_bw=1";
            }

            popup.classList.remove("show");
          }

          YesButton.addEventListener("click", () => confirmConfigure(true));
          NoButton.addEventListener("click", () => confirmConfigure(false));
        });
      </script>
    </form>
  </body>
</html>

<script runat="server">
  protected void Page_Load(object sender, EventArgs e)
  {
      if (!IsPostBack)
      {
          // Retrieve the logged-in username
          string username = GetLoggedInUsername();

          // Display the username
          lblUsername.Text = username;
      }
  }

  protected void btnLogout_Click(object sender, EventArgs e)
  {
      // Log out the user
      Sitecore.Security.Authentication.AuthenticationManager.Logout();

      // Redirect to a logout page or the login page
      Response.Redirect("/sitecore/login?fbc=1");
  }

  private string GetLoggedInUsername()
  {
      // Get the currently logged-in user
      Sitecore.Security.Accounts.User user = Sitecore.Context.User;

      // Get the username
      string username = string.Empty;
      if (user != null)
      {
          if (user.Profile != null && !string.IsNullOrEmpty(user.Profile.FullName))
          {
              username = user.Profile.FullName;
          }
          else
          {
              username = user.Name;
          }
      }
      return username;
  }
</script>
