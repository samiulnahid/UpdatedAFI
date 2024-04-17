<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Vote Result | Armed Forces Insurance</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />
    <!-- CSS -->
    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
  </head>

  <body>
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
    </form>

    <div class="content-container">
      <section class="header-banner">
        <div class="logo">
          <img src="./image/logo.png" alt="Logo" />
        </div>

        <div class="title">
          <h1>Vote Result</h1>
        </div>
      </section>
    </div>

    <!-- ************************************************ -->
    <!--                  Poll  HTML  Start               -->
    <!-- ************************************************ -->
    <div class="content-container" style="display: none !important">
      <section class="poll-section-container" id="poll-section-container">
        <!-- For -->
        <div class="poll-result">
          <h2>Vote 2024 Result - For</h2>
          <div class="poll-container" id="poll-result-for"></div>
        </div>

        <!-- Against -->
        <div class="poll-result">
          <h2>Vote 2024 Result - Against</h2>
          <div class="poll-container" id="poll-result-against"></div>
        </div>
      </section>
    </div>
    <!-- ************************************************ -->
    <!--                  Poll  HTML  End               -->
    <!-- ************************************************ -->

    <div class="content-container">
      <section class="vote-result">
        <div class="button-container">
          <h1 id="vote-title">Vote Result</h1>
          <button class="export-button" id="csvDownloadBtn">Export CSV</button>
        </div>

        <div class="table-container">
          <table>
            <thead>
              <tr>
                <th class="name">Candidate Name</th>
                <th>Number of Votes For</th>
                <th>Number of Votes Against</th>
              </tr>
            </thead>

            <tbody id="tBody"></tbody>
          </table>
        </div>
      </section>
    </div>

    <div class="content-container">
      <section class="voteChart-container">
        <div class="container">
          <canvas id="voteChart"></canvas>
        </div>
      </section>
    </div>

    <div class="content-container">
      <section class="attendChart-container" style="margin-bottom: 50px">
        <div class="container">
          <canvas id="attendChart"></canvas>
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
    <script type="text/javascript" src="/asset/js/chart.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/FileSaver.min.js"></script>

    <script>
      window.addEventListener("DOMContentLoaded", () => {
        const voteTitleEl = document.getElementById("vote-title");
        const tBody = document.getElementById("tBody");
        const ctx = document.getElementById("voteChart");
        const attendChartCtx = document.getElementById("attendChart");

        // ******************* TABLE *******************
        // ******************* TABLE *******************
        function addDataToTable(data = []) {
          tBody.innerHTML = data
            .map((item) => {
              return `<tr>
              <td>${item?.CandidateName}</td>
              <td>${item?.VoteFor}</td>
              <td>${item?.VoteAgainst}</td>
              </tr>`;
            })
            .join("");
        }

        // ******************* GRAPH *******************
        // ******************* GRAPH *******************
        function chartFunctionality(candidates = []) {
          const CandidateNames = candidates?.map((item) => item?.CandidateName);
          const VotesFor = candidates?.map((item) => item?.VoteFor);
          const VotesAgainst = candidates?.map((item) => item?.VoteAgainst);

          const data = {
            labels: CandidateNames,
            datasets: [
              {
                label: "For",
                data: VotesFor,
                backgroundColor: "green",
              },
              {
                label: "Against",
                data: VotesAgainst,
                backgroundColor: "red",
              },
            ],
          };

          // config
          const config = {
            type: "bar",
            data: data,
            options: {
              responsive: true,
              plugins: {
                legend: {
                  position: "top",
                },

                title: {
                  display: true,
                  text: "Vote Result Chart",
                  font: {
                    size: 22,
                    weight: "bold",
                    family: "'Roboto', sans-serif",
                    lineHeight: 1.2,
                  },
                },
              },
              scales: {
                x: {
                  ticks: {
                    font: {
                      size: 12,
                      weight: "bold",
                      family: "'Roboto', sans-serif",
                    },

                    minRotation: 90,
                  },
                },
                y: {
                  ticks: {
                    font: {
                      size: 16,
                      weight: "500",
                      family: "'Roboto', sans-serif",
                    },
                  },
                },
              },
            },
          };

          // chart declaration
          new Chart(ctx, config);
        }

        // ******************* AJAX *******************
        // ******************* AJAX *******************
        // fetch candidate data
        function fetchCandidateData() {
          const url = `/api/sitecore/AFIReport/GetVoteCountReportForResult`;

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

              addDataToTable(parsedData);
              chartFunctionality(parsedData);

              // poll call
              againstPollFunctionality(parsedData);
              forPollFunctionality(parsedData);

              stopLoader();
            },
            error: function (xhr, status, error) {
              alert("No Data Found!");
              stopLoader();
            },
          });
        }
        fetchCandidateData();

        // download scv
        function downloadCSV(e) {
          e.preventDefault();
          const url = `/api/sitecore/AFIReport/DownloadVoteReportAsCSV`;

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
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, "");
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

        /********************************************************
         *                   Vote Attend Graph
         ********************************************************/
        function attendChartFunctionality(parsedData) {
          const LabelsData = [parsedData?.Title];
          const totalMembers = [parsedData?.TotalMembersInVotingPeriod];
          const votedMembers = [parsedData?.TotalEnabledMembers];
          const notVotedMembers = [parsedData?.TotalDisabledMembers];

          const data = {
            labels: LabelsData,
            datasets: [
              {
                label: "Voted Members",
                data: votedMembers,
                backgroundColor: "green",
              },
              {
                label: "Not Voted Members",
                data: notVotedMembers,
                backgroundColor: "red",
              },
            ],
          };

          // config
          const config = {
            type: "bar",
            data: data,
            options: {
              responsive: true,
              plugins: {
                legend: {
                  position: "top",
                },
                title: {
                  display: true,
                  text: "Total Members: " + totalMembers,
                  font: {
                    size: 22,
                    weight: "bold",
                    family: "'Roboto', sans-serif",
                    lineHeight: 1.2,
                  },
                },
              },
              scales: {
                x: {
                  ticks: {
                    font: {
                      size: 20,
                      weight: "bold",
                      family: "'Roboto', sans-serif",
                    },

                    // minRotation: 90,
                  },
                },
                y: {
                  ticks: {
                    font: {
                      size: 16,
                      weight: "500",
                      family: "'Roboto', sans-serif",
                    },
                  },
                },
              },
            },
          };

          // chart declaration
          new Chart(attendChartCtx, config);
        }

        // fetch attend graph data
        function fetchAttendGraphData() {
          const url = `/api/sitecore/AFIReport/GetTotalVotingDetails`;

          $.ajax({
            type: "GET",
            url: url,
            responseType: "json",
            success: function (data, status, xhr) {
              if (!data || data === "") {
                alert("No graph data found.");
                return;
              }

              const parsedData = JSON.parse(data);

              // title update
              const voteTitle = parsedData?.Title;
              voteTitleEl.innerHTML = voteTitle;

              // chart declaration
              attendChartFunctionality(parsedData);
            },
            error: function (xhr, status, error) {
              alert("No graph data found.");
            },
          });
        }

        fetchAttendGraphData();
      });
    </script>

    <!-- ************************************************ -->
    <!--                    Poll Script                   -->
    <!-- ************************************************ -->
    <script>
      function forPollFunctionality(data = []) {
        const pollContainer = document.getElementById("poll-result-for");
        if (!pollContainer) return;

        pollContainer.innerHTML = "";

        data?.forEach((item) => {
          const percentage = parseInt((parseInt(item?.VoteFor) / parseInt(item?.TotalVotes)) * 100);
          let progressColor = "gray";
          if (percentage < 30) progressColor = "red";
          if (percentage > 65) progressColor = "green";

          const pollItemEl = document.createElement("div");
          pollItemEl.className = "progress-bar";

          pollItemEl.innerHTML = `
              <span class="progress" style="width: ${percentage}%; background: ${progressColor};"></span>

              <div class="poll-text">
                <span class="poll-description">${item?.CandidateName}</span>
                <span class="percentage" >${percentage}%</span>
              </div>`;

          pollContainer.appendChild(pollItemEl);
        });
      }

      function againstPollFunctionality(data = []) {
        const pollContainer = document.getElementById("poll-result-against");
        if (!pollContainer) return;

        pollContainer.innerHTML = "";

        data?.forEach((item) => {
          const percentage = parseInt((parseInt(item?.VoteAgainst) / parseInt(item?.TotalVotes)) * 100);

          const pollItemEl = document.createElement("div");
          pollItemEl.className = "progress-bar";

          pollItemEl.innerHTML = `
              <span class="progress" style="width: ${percentage}%"></span>

              <div class="poll-text">
                <span class="poll-description">${item?.CandidateName}</span>
                <span class="percentage" >${percentage}%</span>
              </div>`;

          pollContainer.appendChild(pollItemEl);
        });
      }
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
