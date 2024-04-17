<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="configure-vote.aspx.cs" %>

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
          <a href="/sitecore/shell/sitecore/client/applications/launchpad" title="Back to Lunchpad" class="back">
            <img src="./image/back.svg" alt="back"
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

      <section class="grid-container with__header thank-you-container">
        <div class="box-container">
          <div class="logo-container block">
            <div class="logo">
              <img src="./image/logo.png" alt="Logo" />
            </div>

            <h4>Instruction:</h4>

            <div class="steps">
              <p><b>Voting Period: Lorem ipsum dolor sit amet</b></p>
              <p><b>Candidate: Lorem ipsum dolor sit amet</b></p>
              <p><b>Member: Lorem ipsum dolor sit amet</b></p>
            </div>
          </div>

          <div class="thank-you-message gray__bg">
            <div class="button__container">
              <a href="/sitecore/admin/vote/VotingPeriod.aspx?sc_bw=1" id="vote_page_url" class="button">Vote Period</a>
              <a href="/sitecore/admin/vote/VotingCandidate.aspx?sc_bw=1" class="button">Vote Candidate</a>
              <a href="/sitecore/admin/vote/configure-sync.aspx?sc_bw=1" class="button">Sync Candidate</a>
              <a href="/sitecore/admin/vote/configure-vote-member.aspx?sc_bw=1" class="button">Import Member</a>
              <a href="/sitecore/admin/vote/VotingMember.aspx?sc_bw=1" class="button">Member List</a>
              <a href="/sitecore/admin/vote/VoteTemplate.aspx?sc_bw=1" class="button">Vote Template</a>
              <a href="/sitecore/admin/vote/SyncToSitecoresend.aspx?sc_bw=1" class="button" style="display: none !important;">Sync To Sitecoresend</a>
            </div>
          </div>
        </div>
      </section>
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
