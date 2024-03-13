<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempProspect.aspx.cs"  %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Temp Prospect</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.3.0/css/all.min.css" />
    <link href="~/asset/Fonts/afi-fonts.css" rel="stylesheet" />
    <link href="~/asset/css/common.css" rel="stylesheet" />
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png">

    <style type="text/css">

    body{
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

            .temp_table_body #btn_delete{
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

        .api_result .myPages{
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

        .iplog_title
    {
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


    </style>

    <link type="text/css" rel="stylesheet" href="~/asset/css/pagination/px-pagination.css" />

    <script src="~/asset/js/jsLibrary/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>


</head>

<body>
  
    <div class="container">
    <h1 class="iplog_title">Temp Prospect Data</h1>
    </div>
    <!-- Search result -->
    <section class="search_result" style="display:none;">
        <div class="container">
            <div class="filter_container">

                <!-- Status Dropdown  -->
                <div class="dropdown_container">
                    <label for="search_status">Status:</label>
                    <select id="search_status">
                        <option value="active">Active</option>
                        <option value="pending">Pending</option>
                    </select>
                </div>

                <!-- Email  -->
                <div class="start_date">
                    <label for="search_lead_source">Lead source:</label>
                    <input type="text" id="search_lead_source">
                </div>


                <!-- Coverage  -->
                <div class="end_date">
                    <label for="search_coverage">Coverage:</label>
                    <input type="text" id="search_coverage">
                </div>

                <!-- Search Button  -->
                <button id="btn_search" class="primary_btn search_button">Search</button>
            </div>

        </div>

    </section>

    <!-- Result -->
    <section class="container">

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

                <!-- Status Dropdown  -->
                <div class="dropdown_container">
                    <label for="filter_status">Lead Status:</label>
                    <input type="text" id="filter_lead_status" name="leadStatus" class="input__field">
                </div>

                <!-- synced__filter  -->
                <div class="synced__filter__wrapper">
                    <label for="synced__filter">Sync Filter:</label>
                    <select id="synced__filter" name="syncFilter" class="input__field">
                        <option value="">All</option>
                        <option value="Synced">Synced</option>
                        <option value="Asynced">Asynced</option>
                    </select>
                </div>


                <div class="filter_coverage">
                    <label for="coverage">Coverage:</label>
                    <select id="filter_coverage" name="coverage" class="input__field">
                        <option value="">All</option>
                        <option value="Auto">Auto</option>
                        <option value="Commercial">Business</option>
                        <option value="Collector Vehicle">Collector Vehicle</option>
                        <option value="Home">Home</option>
                        <option value="Flood">Flood</option>
                        <option value="Motorhome">Motorhome</option>
                        <option value="Motorcycle">Motorcycle</option>
                        <option value="Pet Health">Pet Health</option>
                        <option value="Renter">Renter</option>
                        <option value="Travel Trailer">Travel Trailer</option>
                        <option value="Umbrella">Umbrella</option>
                        <option value="Watercraft">Watercraft</option>
                        <option value="Mobile Home">Mobile Home</option>
                        <option value="Dwelling Fire (Landlord)">Dwelling Fire (Landlord)</option>
                        <option value="Condo">Condo</option>
                    </select>
                </div>

            </div>

            <!-- Information Table Start  -->
            <div class="table_container">
                <table>
                    <thead>
                        <tr>
                            <th><input type="checkbox" id="selectAll"> <label for="selectAll">All</label></th>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Mobile Number</th>
                            <th>Address</th>
                            <th>Lead source</th>
                            <th>Coverage</th>
                            <th>Status</th>
                            <th>Synced</th>
                        </tr>
                    </thead>
                    <tbody id="table_body">
                    </tbody>
                </table>
            </div>

            <!-- Pagination buttons will be added here dynamically -->
            <div class="pagination" id="pagination"></div>
    </section>

    <!-- Temporary Table -->
    <section class="container temp_table">
        <!-- Table Start  -->
        <h1 class="page_title">Prospective table</h1>

        <div class="table_container">
            <table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Mobile Number</th>
                        <th>Address</th>
                        <th>Lead source</th>
                        <th>Coverage</th>
                        <th>Status</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody id="temp_table_body">
                </tbody>
            </table>
        </div>

        <div id="temp_pagination" class="pagination">

        </div>


        <!-- Submit Button  -->
        <div class="button_container">
            <button id="btn_submit" class="primary_btn submit_button">Submit</button>
        </div>
    </section>




    <script>
        let selectedData = [];
        const maxPageShowInPagination = 10;
        const urlQuery = {
            page: 1,
            pageSize: 50,
            leadStatus: "",
            syncFilter: "",
            coverage: ""
        }


        $(document).ready(function () {
            fetchDataFromApi();
        });

        // Function to fetch data from API and populate the page
        function fetchDataFromApi() {
            const { page, pageSize, leadStatus, syncFilter, coverage } = urlQuery;
            const url = `/api/sitecore/Prospect/GetTempProspectData?page=${page}&pageSize=${pageSize}&leadStatus=${leadStatus}&syncFilter=${syncFilter}&coverage=${coverage}`

            $.ajax({
                type: "GET",
                url: url,
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: {},
                success: function (data) {
                    // Parse the JSON data
                    var jsonResponse = JSON.parse(data);

                    //  Error Handle
                    if (!jsonResponse?.Success) {
                        var tableBody = $('#table_body');
                        tableBody.empty();

                        document.getElementById("pagination").innerHTML = "";
                        const message = jsonResponse?.Message || "No Data Found!"
                        return alert(message)
                    }

                    var jsonDataList = jsonResponse?.ProspectTempList || [];

                    var tableBody = $('#table_body');
                    tableBody.empty();


                    // Loop through each item in jsonDataList and create table rows
                    $.each(jsonDataList, function (index, item) {
                        var isDisble = item.IsSynced || item.IsBlockCountry;
                        var disabledAttribute = isDisble ? "disabled" : "";
                        var warningMsg = isDisble ? "Data already synced or from a restricted country" : "";

                        const isSelected = selectedData.find(obj => obj.id == item.ID);
                        const checkedAttribute = isSelected ? "checked" : "";

                        var row = $("<tr  title='" + warningMsg + "'></tr>");

                        row.append("<td><input class='checkbox data_checkbox' type='checkbox' data-id='" + item.ID + "'  " + disabledAttribute + " " + checkedAttribute + "></td>");

                        row.append("<td>" + item.FirstName + ' ' + item.LastName + "</td>");
                        row.append("<td>" + item.Email + "</td>");
                        row.append("<td>" + item.Phone + "</td>");
                        row.append("<td>" + item.Address + ', ' + item.City + ', ' + item.State + ', ' + item.ZipCode + "</td>");
                        row.append("<td>" + item.LeadSource + "</td>");
                        row.append("<td>" + item.PreferredCoverage + "</td>");
                        row.append("<td>" + item.LeadStatus + "</td>");
                        row.append("<td>" + item.IsSynced + "</td>");

                        // disbaled checkbox if synced
                        // row.querySelector("checkbox").disbaled = item.IsSynced
                        // Append row to table
                        tableBody.append(row);


                    });

                    storeDataTempTable();
                    // selectedData = [];
                    // tempTable();
                    document.querySelector(`input#selectAll`).checked = false;


                    // Pagination
                    const totalPages = jsonResponse?.TotalPages
                    const CurrentPage = jsonResponse?.CurrentPage

                    const pagination = paginate(totalPages, CurrentPage, maxPageShowInPagination);

                    // Generate pagination HTML with buttons and event listeners
                    const paginationHtml = `
                                <button onclick="handlePaginationClick(${pagination.currentPage - 1})" class="page prev__page" ${pagination.currentPage === 1 ? 'disabled' : ''}><</button>
                                ${pagination.pages.map((page) => {
                        return `<button class="${page === pagination.currentPage ? "active page" : "page"}" onclick="handlePaginationClick(${page})">${page}</button>`;
                    }).join(" ")}
                                <button onclick="handlePaginationClick(${pagination.currentPage + 1})" class="page next__page" ${pagination.currentPage === pagination.totalPages ? 'disabled' : ''}>></button>
                            `;
                    document.getElementById("pagination").innerHTML = paginationHtml;
                },
                error: function (xhr, status, error) {
                    //  Error Handle
                    var tableBody = $('#table_body');
                    tableBody.empty();

                    document.getElementById("pagination").innerHTML = ""

                    alert("No Data Found!")
                }
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

        inputFields.forEach(field => {
            field.addEventListener("change", (e) => {
                const name = e.target.name;
                const value = e.target.value?.trim().toLocaleLowerCase();

                urlQuery[name] = value;
                urlQuery["page"] = 1;
                fetchDataFromApi()
            })
        })
    </script>

    <script>

        // Submit Button Action
        function syncDataFromApi(selectedData) {
            var url = "/api/sitecore/Prospect/SyncTempApprovedtoProspect";

            if (selectedData.length > 0) {
                url += "?ids=" + selectedData;
            }


            $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                success: function (data) {

                    if (data.Success) {
                        const message = data.Message || "Data submitted successfully."
                        alert(message);


                        fetchDataFromApi();

                    } else {
                        const message = data.Message || "Failed to submit!."
                        alert(message);
                    }

                    tempTable(true);

                },
                error: (error) => {
                    const message = data.Message || "Failed to submit!."
                    alert(message)
                }
            });
        }

        document.getElementById('btn_submit').addEventListener('click', () => {



            let selectedDataID = selectedData.map(item => item.id)
            console.log(selectedDataID);
            syncDataFromApi(selectedDataID)

        })



        document.getElementById('btn_search').addEventListener('click', () => {
            const status = document.getElementById('search_status').value;
            const leadSource = document.getElementById('search_lead_source').value;
            const coverage = document.getElementById('search_coverage').value;

            console.log(status, leadSource, coverage);
            // CALL THIS FUNCTION
            searchDataFromApi(status, leadSource, coverage);
        });

        // All Checkbox button functionality
        document.getElementById('selectAll').addEventListener('change', function () {
            var checkboxes = document.querySelectorAll('.checkbox');
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = this.checked && !checkbox.disabled;
                var isChecked = checkbox.checked
                storeSelectedData(checkbox, isChecked)
            }.bind(this));
        });

        function uncheckAllCheckbox() {
            document.getElementById('selectAll').checked = false;
        }

        // unChecked "ALl" checkbox if any checkbox is unchecked
        function unCheckedALLBox() {
            document.querySelectorAll('.checkbox').forEach(checkbox => {
                // console.log(checkbox.checked);
                checkbox.addEventListener('change', () => {
                    if (!checkbox.checked) {
                        document.getElementById('selectAll').checked = false;
                    }
                });
            });
        }
        unCheckedALLBox()


    </script>


    <script>
        // "All" checkbox button functionality
        function storeSelectedData(checkbox, isChecked) {
            var row = checkbox.parentNode.parentNode;
            var rowData = {
                id: checkbox.dataset.id,
                name: row.cells[1].textContent,
                email: row.cells[2].textContent,
                mobile: row.cells[3].textContent,
                address: row.cells[4].textContent,
                leadSource: row.cells[5].textContent,
                coverage: row.cells[6].textContent,
                status: row.cells[7].textContent
            };

            let isPresent = selectedData.find(data => data.id === rowData.id)

            if (isChecked && !isPresent) {
                selectedData.push(rowData);
            }
            else {
                if (!isChecked) {
                    selectedData = selectedData.filter(function (item) {
                        return item.id !== rowData.id;
                    });
                }
            }
            tempTable();
            // console.log(selectedData);
        }

        // Temp table data view
        function storeDataTempTable() {
            let checkboxes = document.querySelectorAll('.data_checkbox');

            checkboxes.forEach(function (checkbox) {
                checkbox.addEventListener('change', function () {

                    var id = this.getAttribute('data-id');
                    var row = this.closest('tr');
                    var rowData = {
                        id: id,
                        name: row.cells[1].textContent,
                        email: row.cells[2].textContent,
                        mobile: row.cells[3].textContent,
                        address: row.cells[4].textContent,
                        leadSource: row.cells[5].textContent,
                        coverage: row.cells[6].textContent,
                        status: row.cells[7].textContent
                    };

                    let isPresent = selectedData.find(data => data.id === id)

                    if (!isPresent && this.checked) {
                        selectedData.push(rowData);
                    }
                    else {
                        if (!this.checked) {
                            selectedData = selectedData.filter(function (item) {
                                return item.id !== id;
                            });
                        }
                    }
                    // console.log(selectedData);
                    tempTable();

                });
            });
        }
        storeDataTempTable();

        window.addEventListener("load", storeDataTempTable)
        window.addEventListener("load", unCheckedALLBox)

    </script>


    <!-- Temp table data load and pagination -->
    <script>
        function tempTable(clearData = false) {
            if (clearData) selectedData = [];

            const tempTable = document.querySelector('.temp_table')
            if (selectedData.length > 0) {
                tempTable.classList.add('active');
            }
            else {
                tempTable.classList.remove('active');
            }

            var tableBody = document.getElementById('temp_table_body');
            tableBody.innerHTML = '';

            selectedData.forEach(function (item) {
                var row = `
                    <tr>
                        <td>${item.name}</td>
                        <td>${item.email}</td>
                        <td>${item.mobile}</td>
                        <td>${item.address}</td>
                        <td>${item.leadSource}</td>
                        <td>${item.coverage}</td>
                        <td>${item.status}</td>
                        <td> <button id="btn_delete" class="" data-id="${item.id}"><i class="fa-solid fa-trash"></i></button></td>
                    </tr>
                `;
                tableBody.insertAdjacentHTML('beforeend', row);
            });

            deleteSelectedItem();
        }


        function deleteSelectedItem() {
            const delete_btn = document.querySelectorAll('#btn_delete')
            delete_btn.forEach(btn => {
                btn.addEventListener('click', () => {
                    // console.log(btn.dataset.id);
                    const selectedID = btn.dataset.id;
                    selectedData = selectedData.filter(function (item) {
                        return item.id !== selectedID;
                    });
                    tempTable();

                    // remove checked from checkbox
                    document.querySelector(`.data_checkbox[data-id='${selectedID}']`).checked = false;
                    document.querySelector(`input#selectAll`).checked = false;
                    // unCheckedALLBox();
                })
            })
        }
    </script>


</body>

</html>

