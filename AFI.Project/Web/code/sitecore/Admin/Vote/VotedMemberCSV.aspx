<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Voted Members download</title>
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
          <h1>Voted Members</h1>
        </div>
      </div>
    </div>

    <!-- Result -->
    <div class="content-container">
      <section class="voting__period__list">
        <div class="api_result" style="background: gray; padding: 33px 0px">
          <!-- <div class="button-container">
            <button class="export-button add-btn button" id="csvDownloadBtn">Export CSV</button>
          </div> -->

          <div
            class="filter_container"
            style="justify-content: center; color: white; padding-top: 21px"
          >
            <!-- synced__filter  -->
            <div class="synced__filter__wrapper">
              <label for="synced__filter">Voting Period:</label>
              <select
                id="synced__filter"
                name="VotingPeriodId"
                class="input__field"
              ></select>
            </div>

            <!-- IsEmail Filter -->
            <div class="synced__filter__wrapper">
              <label for="synced__filter">Voted Member:</label>
              <select id="IsVoted__filter" name="IsVoted" class="input__field">
                <option value="all" selected>All</option>
                <option value="voted">Voted</option>
                <option value="notvoted">Not Voted</option>
              </select>
            </div>

            <div
              class="add-button-container"
              style="text-align: right; margin-bottom: 16px"
            >
              <button class="export-button add-btn button" id="csvDownloadBtn">
                Export CSV
              </button>
              <!-- <button class="export-button add-btn button" id="synceToMoosend">Synce</button> -->
            </div>
          </div>
        </div>
      </section>
    </div>

    <!-- ******************* Loader ******************* -->
    <!-- <div id="page-loader">
      <img src="./image/loader-logo.png" alt="logo" />
    </div> -->

    <script>
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
    </script>

    <script>
      const ddlVotePeriod = document.getElementById("synced__filter");
      const ddlVotePeriodforCreate = document.getElementById("VotingPeriodId");

      function addDataToForm(data = []) {
        ddlVotePeriod.innerHTML =
          `<option value="0">Select VotingPeriod</option>` +
          `<option value="99999999">All VotingPeriod</option>` +
          data
            .map((item) => {
              return `
            <option value="${item?.VotingPeriodId}">${item?.Title}</option>
           `;
            })
            .join("");
      }

      function addDataToFormAdd(data = []) {
        ddlVotePeriodforCreate.innerHTML =
          `<option value>Select One</option>` +
          data
            .map((item) => {
              return `
            <option value="${item?.VotingPeriodId}">${item?.Title}</option>
           `;
            })
            .join("");

        ddlVotePeriodforCreate.required = true;
      }

      // fetch VotingPeriodData data
      function fetchVotingPeriodData() {
        const url = `/api/sitecore/AFIReport/GetAllVotingPeriod`;

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
            addDataToFormAdd(parsedData);

            // stopLoader();
          },
          error: function (xhr, status, error) {
            alert("No Data Found!");
            // stopLoader();
          },
        });
      }
      fetchVotingPeriodData();
    </script>

    <script>
      let selectedData = [];
      const urlQuery = {
        VotingPeriodId: 0,
        IsVoted: "All",
      };

      // Filtering Functionality
      const inputFields = document.querySelectorAll(
        ".filter_container .input__field"
      );

      inputFields.forEach((field) => {
        field.addEventListener("change", (e) => {
          const name = e.target.name;
          const value = e.target.value?.trim().toLocaleLowerCase();

          urlQuery[name] = value;
        });
      });
    </script>

    <script>
      // download scv
      function downloadCSV(e) {
        e.preventDefault();
        const { VotingPeriodId, IsVoted } = urlQuery;

        const url = `/api/sitecore/AFIReport/GetTotalMemberByVoting?&VotingId=${VotingPeriodId}&memberVoted=${IsVoted}`;

        $.ajax({
          type: "GET",
          url: url,
          responseType: "blob",
          success: function (data, status, xhr) {
            if (!data || data === "") {
              alert("No data found.");
              return;
            }
            var filename = "";
            var disposition = xhr.getResponseHeader("Content-Disposition");
            if (disposition && disposition.indexOf("attachment") !== -1) {
              var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
              var matches = filenameRegex.exec(disposition);
              if (matches != null && matches[1])
                filename = matches[1].replace(/['"]/g, "");
            }
            const blob = new Blob([data], { type: "text/csv" });
            saveAs(blob, filename);
          },
          error: function (xhr, status, error) {
            alert("No Data Found!");
          },
        });
      }

      const csvDownloadBtn = document.getElementById("csvDownloadBtn");
      csvDownloadBtn.addEventListener("click", downloadCSV);
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
