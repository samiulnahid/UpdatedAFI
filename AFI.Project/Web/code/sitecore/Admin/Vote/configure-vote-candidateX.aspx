<%@ page language="C#" autoeventwireup="true" codebehind="configure-vote-candidate.aspx.cs" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>AFI Vote Candidate</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />
    <link rel="stylesheet" href="css/vote-panel.css" />
    <link rel="stylesheet" href="icon/css/all.min.css" />

    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
    <script type="text/javascript">
      $(document).ready(function () {
        // Function to fetch data and populate dropdown
        function populateDropdown() {
          $.ajax({
            type: "GET",
            url: "/api/sitecore/AFIReport/GetAllVotingPeriod",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
              var ddlVotePeriod = $("#votingPeriodId");
              ddlVotePeriod.empty(); // Clear existing options
              $.each(data, function (key, value) {
                ddlVotePeriod.append($("<option></option>").attr("value", value.VotingPeriodId).text(value.Title));
              });
            },
            error: function (xhr, status, error) {
              console.error("Error fetching data:", error);
            },
          });
        }

        // Populate dropdown on page load
        populateDropdown();
      });
    </script>

    <script type="text/javascript">
      $(document).ready(function () {
        // Function to load candidate data into the table
        function loadCandidateData() {
          $.ajax({
            type: "GET",
            url: "/api/sitecore/AFIReport/GetAllCandidateData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
              var candidateTable = $("#candidateTable tbody");
              candidateTable.empty();
              $.each(data, function (index, candidate) {
                var row = $("<tr>");
                row.append($("<td>").text(candidate.VotingPeriodName));
                row.append($("<td>").text(candidate.Name));
                row.append($("<td>").text(candidate.Content));
                var editBtn = $("<button>Edit</button>").click(function () {
                  // Handle edit functionality
                  // Hide btnInsert, show btnUpdate
                  $("#btnInsert").hide();
                  $("#btnUpdate").show();
                  // Fill form fields with candidate data
                  $("#votingPeriodId").val(candidate.VotingPeriodId);
                  $("#name").val(candidate.Name);
                  $("#content").val(candidate.Content);
                });
                var deleteBtn = $("<button>Delete</button>").click(function () {
                  // Handle delete functionality
                  var confirmDelete = confirm("Are you sure to delete this?");
                  if (confirmDelete) {
                    $.ajax({
                      type: "DELETE",
                      url: "/api/sitecore/AFIReport/DeleteCandidateData?id=" + candidate.Id,
                      success: function () {
                        // Reload candidate data after successful deletion
                        loadCandidateData();
                      },
                      error: function (xhr, status, error) {
                        console.error("Error deleting candidate:", error);
                      },
                    });
                  }
                });
                row.append($("<td>").append(editBtn).append(deleteBtn));
                candidateTable.append(row);
              });
            },
            error: function (xhr, status, error) {
              console.error("Error loading candidate data:", error);
            },
          });
        }

        // Load candidate data on page load
        loadCandidateData();

        // Insert candidate
        $("#btnInsert").click(function () {
          var votingPeriodId = $("#votingPeriodId").val();
          var name = $("#name").val();
          var content = $("#content").val();

          $.ajax({
            type: "POST",
            url: "/api/sitecore/AFIReport/AddCandidateData",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ VotingPeriodId: votingPeriodId, Name: name, Content: content }),
            success: function () {
              // Reload candidate data after successful insertion
              loadCandidateData();
              // Clear form fields
              $("#votingPeriodId").val("");
              $("#name").val("");
              $("#content").val("");
            },
            error: function (xhr, status, error) {
              console.error("Error inserting candidate:", error);
            },
          });
        });

        // Update candidate
        $("#btnUpdate").click(function () {
          var votingPeriodId = $("#votingPeriodId").val();
          var name = $("#name").val();
          var content = $("#content").val();

          $.ajax({
            type: "PUT",
            url: "/api/sitecore/AFIReport/UpdateCandidateData",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ VotingPeriodId: votingPeriodId, Name: name, Content: content }),
            success: function () {
              // Reload candidate data after successful update
              loadCandidateData();
              // Clear form fields
              $("#votingPeriodId").val("");
              $("#name").val("");
              $("#content").val("");
              // Show btnInsert, hide btnUpdate
              $("#btnInsert").show();
              $("#btnUpdate").hide();
            },
            error: function (xhr, status, error) {
              console.error("Error updating candidate:", error);
            },
          });
        });
      });
    </script>
  </head>
  <body>
    <form id="form1" runat="server">
      <header>
        <a href="/sitecore/shell/sitecore/client/applications/launchpad" title="Back to Lunchpad" class="back">
          <img src="./image/back.svg" alt="back"
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
      <section class="title-logo-section">
        <div class="logo">
          <img src="./image/logo.png" alt="Logo" />
        </div>

        <h1>Candidate</h1>
      </section>
      <section>
        <div>
          <label>Voting Period:</label>
          <!-- <select id="votingPeriodId"  runat="server">
           
            </select> -->
        </div>
        <div>
          <label>Name:</label>
          <input type="text" id="name" name="name" />
        </div>
        <div>
          <label>Content:</label>
          <input type="text" id="content" name="content" />
        </div>
        <div>
          <button type="button" id="btnInsert">Insert</button>
          <button type="button" id="btnUpdate" style="display: none">Update</button>
        </div>
      </section>
      <hr />
      <section>
        <table id="candidateTable">
          <thead>
            <tr>
              <th>Voting Period Name</th>
              <th>Name</th>
              <th>Content</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <!-- Table rows loaded dynamically -->
          </tbody>
        </table>
      </section>
    </form>

    <!-- ******************* Loader ******************* -->
    <div id="page-loader">
      <img src="./image/loader-logo.png" alt="logo" />
    </div>

    <script>
      function stopLoader() {
        const pageLoader = document.getElementById("page-loader");
        if (pageLoader) pageLoader.style.display = "none";
      }

      window.addEventListener("load", stopLoader);
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
