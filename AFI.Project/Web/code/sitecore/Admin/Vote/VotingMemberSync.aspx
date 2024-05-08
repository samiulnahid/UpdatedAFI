<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Synce Member to Moosend</title>
    <link
      rel="icon"
      type="image/png"
      sizes="32x32"
      href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png"
    />

    <!-- CSS -->
    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
    <link rel="stylesheet" href="css/voting-member-page.css" />

    <script
      type="text/javascript"
      src="/asset/js/jsLibrary/jquery.min.js"
    ></script>
    <script
      type="text/javascript"
      src="/asset/js/jsLibrary/FileSaver.min.js"
    ></script>
  </head>

  <body class="voting-member">
    <form id="form1" runat="server">
      <header>
        <div class="icon-container">
          <a
            href="/sitecore/shell/sitecore/client/applications/launchpad"
            title="Back to Lunchpad"
            class="back"
          >
            <img src="./image/back.svg" alt="back"
          /></a>

          <a
            href="/sitecore/admin/vote/configure-vote.aspx?sc_bw=1"
            title="Vote configure"
            class="setting"
          >
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
          <asp:Label
            ID="lblUsername"
            runat="server"
            Text=""
            class="username"
          ></asp:Label>

          <img src="./image/avatar.png" alt="User Photo" class="user-photo" />
        </div>
      </header>
    </form>

    <div class="content-container">
      <div class="header-banner">
        <div class="logo">
          <img src="./image/logo.png" alt="Logo" />
        </div>
        <div class="title">
          <h1>Synce Member to Moosend</h1>
        </div>
      </div>
    </div>

    <!-- Result -->
    <div class="content-container">
      <section class="voting__period__list">
        <div class="api_result">
          <form id="add-form">
            <div
              class="add-button-container"
              style="
                text-align: right;
                margin-bottom: 16px;
                background: #ccc;
                padding-top: 20px;
              "
            >
              <span>Sync Latest Voting Period Members to SitecoreSend: </span>
              <div class="field-control" style="display: inline-block">
                <label for="MemberNumber">Count Number *</label>
                <input
                  type="number"
                  id="CountNumber"
                  name="CountNumber"
                  required
                />
              </div>
              <div class="field-control" style="display: inline-block">
                <label for="ListID">List Id *</label>
                <input type="text" id="ListID" name="ListId" required />
              </div>
              <div
                class="field-control"
                style="display: inline-block; width: 200px"
              >
                <button
                  type="button"
                  class="export-button add-btn button"
                  id="synceToMoosend"
                >
                  Sync
                </button>
              </div>

              <p style="font-size: 11px !important; color: rgb(169 15 43)">
                The data synchronization may take a bit of time, so please check
                SitecoreSend after a while.
              </p>
            </div>
            <!-- <button type="submit" class="submit-btn button" id="form-submit-btn">Submit</button> -->
          </form>
        </div>
      </section>
    </div>

    <!-- ******************* Loader ******************* -->
    <!-- <div id="page-loader">
      <img src="./image/loader-logo.png" alt="logo" />
    </div> -->

    <!-- <script>
      function startLoader() {
        const pageLoader = document.getElementById("page-loader");
        if (pageLoader) pageLoader.style.display = "block";
      }

      function stopLoader() {
        const pageLoader = document.getElementById("page-loader");
        if (pageLoader) pageLoader.style.display = "none";
      }

      function loaderInSubmitBtn(status = false) {
        const submitBtn = document.getElementById("form-submit-btn");

        if (status) {
          submitBtn.disabled = true;
          submitBtn.style.opacity = 0.5;
          submitBtn.innerText = "Submitting...";
        } else {
          submitBtn.disabled = false;
          submitBtn.style.opacity = 1;
          submitBtn.innerText = "Submit";
        }
      }
    </script> -->

    <script>
      function synceMoosend(e) {
        e.preventDefault();

        const count = document.getElementById("CountNumber").value;
        const listId = document.getElementById("ListID").value;

        if (listId.trim() === "") {
          alert("List Id is required.");
          return;
        }

        const url = `/api/sitecore/Audience/SyncVoteMemberToMoosendByCount?count=${count}&listId=${listId}`;

        $.ajax({
          type: "GET",
          url: url,
          dataType: "json",
          contentType: "application/x-www-form-urlencoded; charset=UTF-8",
          data: {},
          success: function (data) {
            var jsonResponse = JSON.parse(data);
            // Check if response has Success field
            if ("Success" in jsonResponse) {
              if (!jsonResponse.Success) {
                alert(jsonResponse.Message);
              }
            } else {
              alert(jsonResponse.Message);
            }
          },
          error: function (xhr, status, error) {
            // Handle error
            alert("Synce Error Found!");
          },
        });
      }

      const synceToMoosend = document.getElementById("synceToMoosend");
      synceToMoosend.addEventListener("click", synceMoosend);
    </script>
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
