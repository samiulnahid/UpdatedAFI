<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Voting Member</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <!-- CSS -->
    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
    <link rel="stylesheet" href="css/voting-member-page.css" />

    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/FileSaver.min.js"></script>
  </head>

  <body class="voting-member">
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
      <div class="header-banner">
        <div class="logo">
          <img src="./image/logo.png" alt="Logo" />
        </div>
        <div class="title">
          <h1>Members</h1>
        </div>
      </div>
    </div>

    <!-- Result -->
    <div class="content-container">
      <section class="voting__period__list">
        <div class="api_result">
          <div class="add-button-container" style="text-align: right; margin-bottom: 16px; background: #ccc">
            <span>Sync Latest Voting Period Members to SitecoreSend: </span>
            <button class="export-button add-btn button" id="synceToMoosend">Sync</button>
            <p style="font-size: 11px !important; color: rgb(169 15 43)">
              The data synchronization may take a bit of time, so please check SitecoreSend after a while.
            </p>
          </div>
          <div class="add-button-container" style="text-align: right; margin-bottom: 16px">
            <button id="add-btn" class="add-btn button">Add</button>
            <button class="export-button add-btn button" id="csvDownloadBtn">Export CSV</button>
            <!-- <button class="export-button add-btn button" id="synceToMoosend">Synce</button> -->
          </div>
          <!-- <div class="button-container">
            <button class="export-button add-btn button" id="csvDownloadBtn">Export CSV</button>
          </div> -->

          <div class="filter_container">
            <!-- Status Dropdown  -->
            <div class="dropdown_container">
              <label for="filter_range">Range:</label>
              <select id="filter_range" name="pageSize" class="input__field">
                <option value="20">20</option>
                <option value="50" selected>50</option>
                <option value="100">100</option>
                <option value="200">200</option>
                <option value="300">300</option>
                <option value="500">500</option>
              </select>
            </div>

            <!-- synced__filter  -->
            <div class="synced__filter__wrapper">
              <label for="synced__filter">Voting Period:</label>
              <select id="synced__filter" name="VotingPeriodId" class="input__field"></select>
            </div>

            <!-- IsEmail Filter -->
            <div class="synced__filter__wrapper">
              <label for="synced__filter">Email Address:</label>
              <select id="IsEmail__filter" name="IsEmail" class="input__field">
                <option value="All" selected>All</option>
                <option value="hasEmail">Yes</option>
                <option value="emptyEmail">No</option>
              </select>
            </div>
          </div>
          <!-- Information Table Start  -->
          <div class="table_container table-style">
            <table>
              <thead>
                <tr>
                  <th>MemberNumber</th>
                  <th>PIN</th>
                  <th>Name</th>
                  <th>Email</th>
                  <th>VotingPeriod</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody id="table_body"></tbody>
            </table>
          </div>

          <!-- Pagination buttons will be added here dynamically -->
          <div class="pagination" id="pagination"></div>
        </div>
      </section>
    </div>

    <section>
      <!-- Add Popup Form -->
      <div class="popup-ui" id="popup">
        <div class="popup-content">
          <span class="close" id="close">&times;</span>

          <form id="add-form">
            <input type="hidden" id="MemberId" name="MemberId" />

            <div class="field-control">
              <label for="MemberNumber">Member Number *</label>
              <input type="number" id="MemberNumber" name="MemberNumber" required />
            </div>

            <div class="field-control">
              <label for="PIN">PIN *</label>
              <input type="text" id="PIN" name="PIN" required />
            </div>

            <div class="field-control">
              <label for="FullName ">Full Name *</label>
              <input type="text" id="FullName" name="FullName" required />
            </div>

            <div class="field-control">
              <label for="EmailAddress">Email Address</label>
              <input type="email" id="EmailAddress" name="EmailAddress" />
            </div>

            <div class="field-control">
              <label for="VotingPeriodId">Voting Period *</label>

              <select id="VotingPeriodId" name="VotingPeriodId" required>
                <!-- <option value="">Select One</option>
                <option value="One">One</option>
                <option value="Two">Two</option> -->
              </select>
            </div>

            <button type="submit" class="submit-btn button" id="form-submit-btn">Submit</button>
          </form>
        </div>
      </div>

      <!-- Delete Popup -->
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
      let selectedData = [];
      const maxPageShowInPagination = 10;
      const urlQuery = {
        page: 1,
        pageSize: 50,
        VotingPeriodId: 0,
        IsEmail: "All",
      };

      $(document).ready(function () {
        fetchMemberData();
      });

      // Function to fetch data from API and populate the page
      function fetchMemberData() {
        const { page, pageSize, VotingPeriodId, IsEmail } = urlQuery;
        const url = `/api/sitecore/AFIReport/GetAllVotingMemberData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}&IsEmail=${IsEmail}`;

        startLoader();

        $.ajax({
          type: "GET",
          url: url,
          dataType: "json",
          contentType: "application/x-www-form-urlencoded; charset=UTF-8",
          data: {},
          success: function (data) {
            // Parse the JSON data
            var jsonResponse = JSON.parse(data);

            //  Error Handle
            if (!jsonResponse?.Success) {
              var tableBody = $("#table_body");
              tableBody.empty();

              document.getElementById("pagination").innerHTML = "";
              const message = jsonResponse?.Message || "No Data Found!";
              stopLoader();
              return alert(message);
            }

            var jsonDataList = jsonResponse?.MemberList || [];

            var tableBody = $("#table_body");
            tableBody.empty();

            // Loop through each item in jsonDataList and create table rows
            $.each(jsonDataList, function (index, item) {
              var row = $("<tr data-id='" + item?.MemberId + "'></tr>");

              // row.append("<td>" + item.MemberId || "" + "</td>");
              row.append("<td>" + item.MemberNumber || "" + "</td>");
              row.append("<td>" + item.PIN || "" + "</td>");
              row.append("<td>" + item?.FullName || "" + "</td>");
              // row.append("<td>" + item?.EmailAddress || "" + "</td>");
              row.append("<td>" + (item?.EmailAddress ? item?.EmailAddress : "") + "</td>");
              row.append("<td data-VotingPeriodId='" + item.VotingPeriodId + "'>" + item.VotingPeriod || "" + "</td>");

              // action
              const actionCell = document.createElement("td");
              actionCell.classList.add("action");

              const deleteBtn = document.createElement("button");
              deleteBtn.classList.add("delete-btn");
              deleteBtn.setAttribute("data-id", item.MemberId);
              deleteBtn.innerHTML = '<i class="fa-solid fa-trash-can fa-fw"></i>';
              deleteBtn.addEventListener("click", () => handleDeleteButtonClick(item.MemberId));
              if (item?.IsActive) deleteBtn.disabled = true;
              actionCell.appendChild(deleteBtn);

              const editBtn = document.createElement("button");
              editBtn.classList.add("edit-btn");
              editBtn.setAttribute("data-id", item.MemberId);
              editBtn.innerHTML = '<i class="fa-regular fa-pen-to-square fa-fw"></i>';
              editBtn.addEventListener("click", () => handleEditButtonClick(item.MemberId));
              actionCell.appendChild(editBtn);

              row.append(actionCell);

              // Append row to table
              tableBody.append(row);
            });

            // document.querySelector(`input#selectAll`).checked = false;

            // Pagination
            const totalPages = jsonResponse?.TotalPages;
            const CurrentPage = jsonResponse?.CurrentPage;

            const pagination = paginate(totalPages, CurrentPage, maxPageShowInPagination);

            // Generate pagination HTML with buttons and event listeners
            const paginationHtml = `
                                <button onclick="handlePaginationClick(${pagination.currentPage - 1})" class="page prev__page" ${
              pagination.currentPage === 1 ? "disabled" : ""
            }><</button>
                                ${pagination.pages
                                  .map((page) => {
                                    return `<button class="${
                                      page === pagination.currentPage ? "active page" : "page"
                                    }" onclick="handlePaginationClick(${page})">${page}</button>`;
                                  })
                                  .join(" ")}
                                <button onclick="handlePaginationClick(${pagination.currentPage + 1})" class="page next__page" ${
              pagination.currentPage === pagination.totalPages ? "disabled" : ""
            }>></button>
                            `;
            document.getElementById("pagination").innerHTML = paginationHtml;
            stopLoader();
          },
          error: function (xhr, status, error) {
            //  Error Handle
            var tableBody = $("#table_body");
            tableBody.empty();

            document.getElementById("pagination").innerHTML = "";

            alert("No Data Found!");
            stopLoader();
          },
        });
      }
    </script>

    <!-- *************************** PAGINATION AND FILTERING *************************** -->
    <script>
      function paginate(totalPages, currentPage, displayRange) {
        // Ensure currentPage is within bounds
        if (currentPage < 1) {
          currentPage = 1;
        } else if (currentPage > totalPages) {
          currentPage = totalPages;
        }

        // Calculate start and end page indices
        let startPage, endPage;
        if (totalPages <= displayRange) {
          // When total pages are less than or equal to the display range,
          // display all pages
          startPage = 1;
          endPage = totalPages;
        } else {
          // Calculate start and end page based on current page position
          let halfDisplay = Math.floor(displayRange / 2);

          // Calculate start and end offsets
          let startOffset = currentPage - halfDisplay;
          let endOffset = currentPage + halfDisplay;

          // Ensure start and end offsets are within bounds
          if (startOffset < 1) {
            startOffset = 1;
            endOffset = displayRange;
          } else if (endOffset > totalPages) {
            endOffset = totalPages;
            startOffset = totalPages - displayRange + 1;
          }

          // Set start and end pages
          startPage = startOffset;
          endPage = Math.min(startOffset + displayRange - 1, totalPages); // Limit endPage to displayRange
        }

        // Generate array of page numbers
        let pages = Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);

        const pagesWithFirstAndLast = [...new Set([1, ...pages, totalPages])];

        return {
          currentPage: currentPage,
          totalPages: totalPages,
          pages: pagesWithFirstAndLast,
        };
      }

      // Function to handle pagination button click
      function handlePaginationClick(page) {
        urlQuery.page = page;
        fetchMemberData();
      }

      // Filtering Functionality
      const inputFields = document.querySelectorAll(".filter_container .input__field");

      inputFields.forEach((field) => {
        field.addEventListener("change", (e) => {
          const name = e.target.name;
          const value = e.target.value?.trim().toLocaleLowerCase();

          urlQuery[name] = value;
          urlQuery["page"] = 1;
          fetchMemberData();
        });
      });
    </script>

    <!-- ******************* CREATE, DELETE, UPDATE ******************* -->
    <script>
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
        const { page, pageSize, VotingPeriodId, IsEmail } = urlQuery;

        const url = `/api/sitecore/AFIReport/DeleteMemberData?id=${id}&page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}&IsEmail=${IsEmail}`;

        fetch(url, { method: "POST" })
          .then((response) => {
            if (!response.ok) {
              throw new Error("Failed to delete item.");
            }
            fetchMemberData();
            return response.json();
          })
          .then((data) => {
            const parsedData = JSON.parse(data);
            if (parsedData?.Message) {
              // alert(parsedData?.Message);
            } else {
              alert("Deleted Successfully.");
            }
          })
          .catch((error) => {
            alert(error.message);
          });
      }

      function populateFormFields(row) {
        document.getElementById("MemberId").value = row.dataset.id;
        document.getElementById("MemberNumber").value = row.cells[0].textContent;
        document.getElementById("PIN").value = row.cells[1].textContent;
        document.getElementById("FullName").value = row.cells[2].textContent;
        document.getElementById("EmailAddress").value = row.cells[3].textContent;
        document.getElementById("VotingPeriodId").value = row.cells[4].dataset.votingperiodid;
      }

      document.getElementById("add-form").addEventListener("submit", function (event) {
        event.preventDefault();
        handleFormSubmit();
      });

      // add or update form submit
      function handleFormSubmit() {
        const form = document.getElementById("add-form");
        const formData = new FormData(form);

        const { page, pageSize, VotingPeriodId, IsEmail } = urlQuery;

        const url = formData.get("MemberId")
          ? `/api/sitecore/AFIReport/UpdateMemberData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}&IsEmail=${IsEmail}`
          : `/api/sitecore/AFIReport/InsertMembereData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}&IsEmail=${IsEmail}`;

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
            fetchMemberData();
            form.reset();
            document.getElementById("MemberId").value = "";
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
        document.getElementById("MemberId").value = "";

        document.getElementById("popup").classList.add("show");
      });

      // popup close
      document.getElementById("close").addEventListener("click", function () {
        document.getElementById("popup").classList.remove("show");
        document.getElementById("add-form").reset();
      });
    </script>

    <script>
      // download scv
      function downloadCSV(e) {
        e.preventDefault();
        const { page, pageSize, VotingPeriodId, IsEmail } = urlQuery;

        const url = `/api/sitecore/AFIReport/DownloadMemberFilterDataCSV?VotingId=${VotingPeriodId}&IsEmail=${IsEmail}`;

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
    </script>

    <script>
      // download scv
      function synceMoosend(e) {
        e.preventDefault();
        //  const { page, pageSize, VotingPeriodId, IsEmail } = urlQuery;

        const url = `/api/sitecore/Audience/SyncVoteMemberToMoosend`;

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
