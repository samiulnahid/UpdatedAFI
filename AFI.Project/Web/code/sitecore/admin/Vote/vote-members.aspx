<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>Vote Members</title>

    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />

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

      #selectAll,
      #filter_status {
        cursor: pointer;
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

      .header-banner {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 20px;
        margin-bottom: 50px;
        color: #333;
      }

      .logo img {
        max-width: 100%; /* Ensure the logo doesn't exceed the container width */
        height: auto;
      }

      .title {
        flex-grow: 1; /* Allow the title to grow and occupy remaining space */
        text-align: right;
      }

      .title h1 {
        font-weight: 600 !important;
      }

      @media (max-width: 768px) {
        header {
          flex-direction: column;
          text-align: center;
        }

        .title {
          text-align: center;
          margin-top: 10px; /* Add some space between logo and title on smaller screens */
        }
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
              <option value="20" selected>20</option>
              <option value="50">50</option>
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
        </div>

        <!-- Information Table Start  -->
        <div class="table_container table-style">
          <table>
            <thead>
              <tr>
                <!-- <th><input type="checkbox" id="selectAll" /> <label for="selectAll">All</label></th> -->
                <!-- <th  id="selectAll">MemberId</th> -->
                <th>MemberNumber</th>
                <th>PIN</th>
                <th>Name</th>
                <th>Voting Period</th>
              </tr>
            </thead>
            <tbody id="table_body"></tbody>
          </table>
        </div>

        <!-- Pagination buttons will be added here dynamically -->
        <div class="pagination" id="pagination"></div>
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

    <script>
      const ddlVotePeriod = document.getElementById("synced__filter");

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
      let selectedData = [];
      const maxPageShowInPagination = 10;
      const urlQuery = {
        page: 1,
        pageSize: 20,
        VotingPeriodId: 0,
      };

      $(document).ready(function () {
        fetchDataFromApi();
      });

      // Function to fetch data from API and populate the page
      function fetchDataFromApi() {
        const { page, pageSize, VotingPeriodId } = urlQuery;
        const url = `/api/sitecore/AFIReport/GetAllVotingMemberData?page=${page}&pageSize=${pageSize}&VotingPeriodId=${VotingPeriodId}`;

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
              // const isSelected = selectedData.find((obj) => obj.id == item.MemberId);
              // const checkedAttribute = isSelected ? "checked" : "";
              const warningMsg = "";

              var row = $("<tr  title='" + warningMsg + "'></tr>");

              // row.append(
              //   "<td><input class='checkbox data_checkbox' type='checkbox' data-id='" +
              //     item.MemberId +
              //     "'  " +
              //     "></td>"
              // );
              // row.append("<td>" + item.MemberId || "" + "</td>");
              row.append("<td>" + item.MemberNumber || "" + "</td>");
              row.append("<td>" + item.PIN || "" + "</td>");
              row.append("<td>" + item.FullName || "" + "</td>");
              row.append("<td>" + item.VotingPeriod || "" + "</td>");

              // disbaled checkbox if synced
              // row.querySelector("checkbox").disbaled = item.IsSynced
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
        fetchDataFromApi();
      }

      // Filtering Functionality
      const inputFields = document.querySelectorAll(".filter_container .input__field");

      inputFields.forEach((field) => {
        field.addEventListener("change", (e) => {
          const name = e.target.name;
          const value = e.target.value?.trim().toLocaleLowerCase();

          urlQuery[name] = value;
          urlQuery["page"] = 1;
          fetchDataFromApi();
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
