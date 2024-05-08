<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="configure-member-sync.aspx.cs" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>AFI Vote Member</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
  </head>
  <body>
    <form id="form1" runat="server">
      <header>
        <a href="/sitecore/shell/sitecore/client/applications/launchpad" title="Back to Lunchpad" class="back"
          ><img src="./image/back.svg" alt="back"
        /></a>

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

      <div class="content-container">
        <section class="header-banner">
          <div class="logo">
            <img src="./image/logo.png" alt="Logo" />
          </div>

          <div class="title">
            <h1>Import Member</h1>
          </div>
        </section>

        <section id="add-import-form">
          <div class="field-control">
            <label for="votingPeriod">Voting Period:</label>
            <select id="ddlVotePeriod" required>
              <option value="">Select Voting Period</option>
            </select>
          </div>

          <div class="field-control">
            <label for="file">Import:</label>
            <input type="file" id="fileInput" required />
          </div>

          <div class="submit-btn-container">
            <button type="submit" class="submit-btn button" id="importBtn">Submit</button>
          </div>
        </section>
      </div>

      <!-- ******************* Loader ******************* -->
      <div id="page-loader">
        <img src="./image/loader-logo.png" alt="logo" />
      </div>

      <script>
        function startLoader() {
          const pageLoader = document.getElementById("page-loader");
          if (pageLoader) pageLoader.style.display = "block";
        }

        function stopLoader() {
          const pageLoader = document.getElementById("page-loader");
          if (pageLoader) pageLoader.style.display = "none";
        }
      </script>

      <!-- ************************************************ -->
      <!--                       SCRIPT                     -->
      <!-- ************************************************ -->
      <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
      <script>
        const ddlVotePeriod = document.getElementById("ddlVotePeriod");

        function addDataToForm(data = []) {
          ddlVotePeriod.innerHTML =
            `<option value>Select VotingPeriod</option>` +
            data
              .map((item) => {
                return `
           <option value="${item?.VotingPeriodId}">${item?.Title}</option>
          `;
              })
              .join("");
        }
        // ******************* AJAX *******************
        // fetch VotingPeriodData data
        function fetchVotingPeriodData() {
          const url = `/api/sitecore/AFIReport/GetAllVotingPeriod`;

          startLoader();

          $.ajax({
            type: "GET",
            url: url,
            responseType: "json",
            success: function (data, status, xhr) {
              if (!data || data === "") {
                alert("No data found.");
                return;
              }

              const parsedData = JSON.parse(data);

              addDataToForm(parsedData);

              stopLoader();
            },
            error: function (xhr, status, error) {
              alert("No Data Found!");
              stopLoader();
            },
          });
        }
        fetchVotingPeriodData();
      </script>

      <script>
        function loaderInSubmitBtn(status = false) {
          const submitBtn = document.getElementById("importBtn");

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

        $(document).ready(function () {
          $("#importBtn").click(function () {
            var formData = new FormData();
            var file = $("#fileInput")[0].files[0];
            var dropdownValue = $("#ddlVotePeriod").val();

            if (!dropdownValue || !file) {
              alert("Please fill the form");
              return;
            }

            formData.append("file", file);
            formData.append("dropdownValue", dropdownValue);

            loaderInSubmitBtn(true);

            $.ajax({
              url: "/api/sitecore/AFIReport/SubmitMemberVote",
              type: "POST",
              data: formData,
              contentType: false,
              processData: false,
              success: function (response) {
                alert("Successfully Inserted!");
                console.log("Success:", response);
                loaderInSubmitBtn(false);
              },
              error: function (error) {
                alert("Error!");
                console.error("Error:", error);
                loaderInSubmitBtn(false);
              },
            });
          });
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
      Response.Redirect("/login.aspx");
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
