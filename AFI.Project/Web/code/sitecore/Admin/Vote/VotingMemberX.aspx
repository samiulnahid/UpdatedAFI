<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>Voting Member</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <!-- CSS -->
    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />

    <style type="text/css">
      body {
        margin-bottom: 20px;
      }

      .container {
        max-width: 1600px;
        margin: 0 auto;
      }

      .img_container img {
        display: block;
        margin: 0 auto 60px;
      }

      .page_title {
        text-align: left;
      }

      .table_container {
        overflow-x: auto;
      }

      .table_container table {
        width: 100%;
        border-collapse: collapse;
      }

      #selectAll,
      #filter_status {
        cursor: pointer;
      }

      .table_container th,
      .table_container td {
        padding: 8px;
        text-align: left;
        border-bottom: 1px solid #ddd;
        line-height: 26px;
        width: 20%;
      }

      .table_container th {
        background-color: #f2f2f2;
      }

      .filter_container {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
        align-items: flex-start;
        justify-content: flex-end;
        margin-bottom: 10px;
      }

      .search_result .filter_container {
        justify-content: flex-start;
        margin-bottom: 20px;
      }

      .api_result,
      .search_result {
        margin-bottom: 50px;
      }

      .temp_table,
      .button_container {
        margin-top: 20px;
      }

      .primary_btn {
        background-color: #007bff;
        color: #fff;
        cursor: pointer;
        width: 140px;
        padding: 11px !important;
        font-size: 15px;
      }

      .primary_btn:hover {
        background-color: #0056b3;
      }

      .temp_table_body #btn_delete {
        font-size: 16px;
      }

      #filter_range,
      #search_status,
      #filter_status,
      input[type="text"],
      input[type="date"],
      input[type="email"],
      .primary_btn {
        padding: 8px;
        border-radius: 5px;
        border: 1px solid #ccc;
      }

      .api_result .myPages {
        margin: 30px auto !important;
      }

      .temp_table {
        display: none;
      }

      .temp_table.active {
        display: block;
      }

      .jqp-active {
        color: #0056b3;
        background-color: #fff;
        border: 1px solid #0056b3;
        font-weight: 700;
      }

      .jqp-active:hover {
        color: #0056b3;
        background-color: #fff;
        border: 1px solid #0056b3;
        font-weight: 700;
      }

      @media screen and (max-width: 575px) {
        .filter_container {
          justify-content: flex-start;
        }
      }

      .iplog_title {
        margin: 50px auto !important;
        font-weight: 600 !important;
      }

      .pagination {
        display: flex;
        justify-content: center;
        margin-top: 20px;
        user-select: none;
        gap: 10px;
      }

      .pagination .prev__page,
      .pagination .next__page,
      .pagination .page {
        padding: 10px;
        color: #0056b3;
        background-color: #fff;
        border: 1px solid #0056b3;
        border-radius: 3px;
        cursor: pointer;
        font-size: 16px;
      }

      .pagination .page:hover {
        background-color: #0056b3;
        color: #fff;
      }

      .pagination .active {
        background-color: #007bff;
        color: #fff;
        border: none;
        font-weight: 700;
      }

      .pagination .active:hover {
        color: #0056b3;
        background-color: #fff;
        border: 1px solid #0056b3;
        font-weight: 700;
      }

      .fa-trash:before {
        font-size: 17px;
      }

      .download__container {
        text-align: right;
        margin-bottom: 40px;
      }
      .download__container a {
        display: inline-flex;
        gap: 5px;
        align-items: center;
        padding: 10px;
      }
    </style>

    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/FileSaver.min.js"></script>
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
    <section class="content-container">
      <div class="api_result">
        <!-- <h1 class="page_title">Result</h1> -->

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

          <div class="add-button-container">
            <button id="add-btn" class="add-btn button">Add</button>
          </div>
        </div>

        <!-- Information Table Start  -->
        <div class="table_container">
          <table>
            <thead>
              <tr>
                <!-- <th><input type="checkbox" id="selectAll" /> <label for="selectAll">All</label></th> -->
                <!-- <th  id="selectAll">MemberId</th> -->
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
              <input type="password" id="PIN" name="PIN" required />
            </div>

            <div class="field-control">
              <label for="FullName ">Full Name *</label>
              <input type="text" id="FullName" name="FullName" required />
            </div>

            <div class="field-control">
              <label for="EmailAddress">Email Address *</label>
              <input type="email" id="EmailAddress" name="EmailAddress" required />
            </div>

            <div class="field-control">
              <label for="VotingPeriodId">Voting Period *</label>
              <select id="VotingPeriodId" name="VotingPeriodId" required>
                <!-- <option value="">Select One</option>
                <option value="One">One</option>
                <option value="Two">Two</option> -->
              </select>
            </div>

            <button type="submit" class="submit-btn button">Submit</button>
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
      function stopLoader() {
        const pageLoader = document.getElementById("page-loader");
        if (pageLoader) pageLoader.style.display = "none";
      }
    </script>

    <!-- ************************************************ -->
    <!--                      SCRIPT                      -->
    <!-- ************************************************ -->
    <!-- ************ FIELD DYNAMIC OPTION ********** -->
    <script>
      const ddlVotePeriod = document.getElementById("synced__filter");
      const ddlVotePeriodforCreate = document.getElementById("VotingPeriodId");

      function addDataToForm(data = []) {
        ddlVotePeriod.innerHTML =
          `<option value="0">All VotingPeriod</option>` +
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
      };

      $(document).ready(function () {
        fetchMemberData();
      });

      // Function to fetch data from API and populate the page
      function fetchMemberData() {
        const { page, pageSize, VotingPeriodId } = urlQuery;
        const url = `/api/sitecore/AFIReport/GetAllVotingMemberData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}`;

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
              row.append("<td>" + item.FullName || "" + "</td>");
              row.append("<td >" + item.EmailAddress || "" + "</td>");
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
          },
          error: function (xhr, status, error) {
            //  Error Handle
            var tableBody = $("#table_body");
            tableBody.empty();

            document.getElementById("pagination").innerHTML = "";

            alert("No Data Found!");
          },
        });
      }
    </script>

    <!-- ****************** PAGINATION AND FILTERING ****************** -->
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
        const { page, pageSize, VotingPeriodId } = urlQuery;

        const url = `/api/sitecore/AFIReport/DeleteMemberData?id=${id}&page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}`;

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

        const { page, pageSize, VotingPeriodId } = urlQuery;

        const url = formData.get("MemberId")
          ? `/api/sitecore/AFIReport/UpdateMemberData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}`
          : `/api/sitecore/AFIReport/InsertMembereData?page=${page}&pageSize=${pageSize}&VotingId=${VotingPeriodId}`;

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
          });
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
