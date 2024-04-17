<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="configure.aspx.cs" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <meta charset="UTF-8" />
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>Candidate</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <!-- CSS -->
    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
  </head>

  <body class="">
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
      <!-- ******************* Title and Log ******************* -->
      <div class="header-banner">
        <div class="logo">
          <img src="./image/logo.png" alt="Logo" />
        </div>
        <div class="title">
          <h1>Candidate</h1>
        </div>
      </div>

      <!-- ******************* Voting Period List ******************* -->
      <section class="voting__period__list">
        <div class="add-btn-container">
          <button class="button" id="add-btn">Add</button>
        </div>

        <div class="table-container table-style">
          <table>
            <thead>
              <tr>
                <th>Voting Period</th>
                <th>Name</th>
                <th>Content</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody id="tbody">
              <!-- Dynamic data here -->
            </tbody>
          </table>
        </div>
      </section>
    </div>

    <!-- Add Popup Form -->
    <div class="popup-ui" id="popup">
      <div class="popup-content">
        <span class="close" id="close">&times;</span>

        <form id="add-form">
          <input type="hidden" id="CandidateId" name="CandidateId" />

          <div class="field-control">
            <label for="VotingPeriodId">Voting Period *</label>
            <select type="text" id="VotingPeriodId" name="VotingPeriodId" required>
              <!-- <option value="">Select One</option>
                  <option value="One">One</option>
                  <option value="Two">Two</option> -->
            </select>
          </div>

          <div class="field-control">
            <label for="Name">Name *</label>
            <input type="text" id="Name" name="Name" required />
          </div>

          <div class="field-control">
            <label for="Content">Content</label>
            <textarea
              id="Content"
              name="Content"
              style="height: 100px; width: 100%; border: 1px solid #000; padding: 5px"
            ></textarea>
          </div>

          <button type="submit" class="submit-btn button" id="form-submit-btn">Submit</button>
        </form>
      </div>
    </div>

    <div class="popup-ui" id="deletePopup">
      <div class="popup-content delete-warning">
        <span class="close" id="deletePopupClose">&times;</span>

        <h3>Are you sure to <b>Delete</b></h3>

        <div class="button-container">
          <button id="delete-no-button">No</button>
          <button id="delete-yes-button">Yes</button>
        </div>
      </div>
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

    <!-- ************************************************ -->
    <!--                      SCRIPT                      -->
    <!-- ************************************************ -->

    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>

    <!-- ************************************************ -->
    <!--                LOAD Voting Period For Dropdown         -->
    <!-- ************************************************ -->
    <script>
      const ddlVotePeriod = document.getElementById("VotingPeriodId");

      function addDataToForm(data = []) {
        ddlVotePeriod.innerHTML =
          `<option value="0">Select One</option>` +
          data
            .map((item) => {
              return `
            <option value="${item?.VotingPeriodId}">${item?.Title}</option>
           `;
            })
            .join("");
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
          },
          error: function (xhr, status, error) {
            alert("No Data Found!");
          },
        });
      }
      fetchVotingPeriodData();
    </script>

    <script>
      document.addEventListener("DOMContentLoaded", function () {
        fetchCandidateData();

        function fetchCandidateData() {
          const url = "/api/sitecore/AFIReport/GetAllCandidateData";

          startLoader(url);

          fetch(url)
            .then((response) => {
              if (!response.ok) {
                throw new Error("Failed to fetch data.");
              }
              return response.json();
            })
            .then((data) => {
              const parsedData = JSON.parse(data);

              if (!parsedData || parsedData.length === 0) {
                alert("No data found.");
                return;
              }
              addDataToTable(parsedData);
            })
            .catch((error) => {
              alert("Error fetching data: " + error.message);
            })
            .finally(() => stopLoader());
        }

        function addDataToTable(data) {
          const tbody = document.getElementById("tbody");
          tbody.innerHTML = "";

          data.forEach((item) => {
            const row = document.createElement("tr");
            row.setAttribute("data-id", item.CandidateId);

            const actionCell = document.createElement("td");
            actionCell.classList.add("action");

            const deleteBtn = document.createElement("button");
            deleteBtn.classList.add("delete-btn");
            deleteBtn.setAttribute("data-id", item.CandidateId);
            deleteBtn.innerHTML = '<i class="fa-solid fa-trash-can fa-fw"></i>';
            deleteBtn.addEventListener("click", () => handleDeleteButtonClick(item.CandidateId));
            if (item?.IsActive) deleteBtn.disabled = true;
            actionCell.appendChild(deleteBtn);

            const editBtn = document.createElement("button");
            editBtn.classList.add("edit-btn");
            editBtn.setAttribute("data-id", item.CandidateId);
            editBtn.innerHTML = '<i class="fa-regular fa-pen-to-square fa-fw"></i>';
            editBtn.addEventListener("click", () => handleEditButtonClick(item.CandidateId));
            actionCell.appendChild(editBtn);

            row.innerHTML = `
            <td style="display:none;">${item?.VotingPeriodId || ""}</td>
           <td>${item?.VotingPeriodName || ""}</td>
          <td>${item?.Name || ""}</td>
          <td>${item?.Content || ""}</td>
        `;
            row.appendChild(actionCell);

            tbody.appendChild(row);
          });
        }

        function handleEditButtonClick(id) {
          const row = document.querySelector(`tr[data-id="${id}"]`);
          populateFormFields(row);
          document.getElementById("popup").classList.add("show");
        }

        function handleDeleteButtonClick(id) {
          const confirmDelete = confirm("Are you sure you want to delete this item?");
          if (confirmDelete) {
            deleteItem(id);
          }
        }

        function deleteItem(id) {
          const url = `/api/sitecore/AFIReport/DeleteCandidateData?id=${id}`;
          fetch(url, { method: "POST" })
            .then((response) => {
              if (!response.ok) {
                throw new Error("Failed to delete item.");
              }
              fetchCandidateData();
              return response.json();
            })
            .then((data) => {
              const parsedData = JSON.parse(data);
              if (parsedData?.Message) {
                alert(parsedData?.Message);
              } else {
                alert("Deleted Successfully.");
              }
            })
            .catch((error) => {
              alert(error.message);
            });
        }

        function populateFormFields(row) {
          document.getElementById("CandidateId").value = row.dataset.id;
          document.getElementById("VotingPeriodId").value = row.cells[0].textContent;
          document.getElementById("Name").value = row.cells[2].textContent;
          document.getElementById("Content").value = row.cells[3].textContent;
        }

        document.getElementById("add-form").addEventListener("submit", function (event) {
          event.preventDefault();
          handleFormSubmit();
        });

        // add or update form submit
        function handleFormSubmit() {
          const form = document.getElementById("add-form");
          const formData = new FormData(form);

          const url = formData.get("CandidateId")
            ? "/api/sitecore/AFIReport/UpdateCandidateData"
            : "/api/sitecore/AFIReport/AddCandidateData";

          loaderInSubmitBtn(true);

          fetch(url, {
            method: "POST",
            body: formData,
          })
            .then((response) => {
              if (!response.ok) {
                throw new Error("Failed to submit form.");
              }
              document.getElementById("popup").classList.remove("show");
              fetchCandidateData();
              form.reset();
              document.getElementById("CandidateId").value = "";
              return response.json();
            })
            .then((data) => {
              const parsedData = JSON.parse(data);
              if (parsedData?.Message) {
                alert(parsedData?.Message);
              } else {
                alert("Submitted Successfully.");
              }
            })
            .catch((error) => {
              alert(error.message);
            })
            .finally(() => loaderInSubmitBtn(false));
        }

        // Add functionality to the "add-btn"
        document.getElementById("add-btn").addEventListener("click", function () {
          document.getElementById("add-form").reset();
          document.getElementById("CandidateId").value = "";

          document.getElementById("popup").classList.add("show");
        });

        // popup close
        document.getElementById("close").addEventListener("click", function () {
          document.getElementById("popup").classList.remove("show");
          document.getElementById("add-form").reset();
        });
      });
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
