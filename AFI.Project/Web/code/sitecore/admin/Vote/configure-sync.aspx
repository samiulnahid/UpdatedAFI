<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="configure-sync.aspx.cs" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>Sync Candidate</title>
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
          <div class="logo-container">
            <div class="logo">
              <img src="./image/logo.png" alt="Logo" />
            </div>

            <h4>If you want to configure candidate details, please follow these steps:</h4>

            <div class="steps">
              <p><b>Step 01: </b>Go to the Content Editors panel</p>
              <p><b>Step 02: </b>Navigate to /sitecore/content/AFI/afi/Data/Proxy Vote/Proxy Vote Candidates.</p>
              <p><b>Step 03: </b>Select Candidate and update the Name URL.</p>
            </div>
          </div>

          <div class="thank-you-message">
            <p>Select the Voting Period first, then click the sync button.</p>
            <p class="note">
              <b>Note:</b> After syncing, the current candidates will be removed, and new candidates from the selected Voting
              Period will be created in Sitecore.
            </p>

            <section>
              <div id="v_period">
                <select id="ddlVotePeriod">
                  <option value>Select Voting Period</option>
                </select>
              </div>

              <button type="submit" class="button" id="configureBtn">Sync Candidate</button>
            </section>
          </div>
        </div>
      </section>

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
      <!--                        SCRIPT                    -->
      <!-- ************************************************ -->
      <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
      <script>
        const ddlVotePeriod = document.getElementById("ddlVotePeriod");

        // function addDataToForm(data = []) {
        //   ddlVotePeriod.innerHTML = data
        //     .map((item) => {
        //       return `
        //       <option value="${item?.VotingPeriodId}">${item?.Title}</option>
        //      `;
        //     })
        //     .join("");
        // }

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
          const submitBtn = document.getElementById("configureBtn");

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
          $("#configureBtn").click(function (e) {
            e.preventDefault(); // Prevent the default form submission

            const periodId = document.getElementById("ddlVotePeriod").value;

            // Check if periodId is not null or empty
            if (periodId && periodId.trim() !== "") {
              loaderInSubmitBtn(true);

              $.ajax({
                type: "POST",
                url: "/api/sitecore/AFIReport/CreateCandidteItem",
                data: { periodId: periodId }, // Send periodId as a parameter
                dataType: "json",
                success: function (data, status, xhr) {
                  if (data && data.Success) {
                    // Data successfully processed
                    alert("Data successfully processed.");
                    // Add your code here if needed
                  } else {
                    // No data or operation failed
                    alert("Data successfully processed.");
                  }

                  loaderInSubmitBtn();
                },
                error: function (xhr, status, error) {
                  // Error occurred during the AJAX request
                  alert("An error occurred during the request.");
                  loaderInSubmitBtn();
                },
              });
            } else {
              // Alert if periodId is null or empty
              alert("Please select a voting period.");
            }
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
