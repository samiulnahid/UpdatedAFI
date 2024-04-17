<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Vote Result | Armed Forces Insurance</title>

    <!-- CSS -->
    <link rel="stylesheet" href="~/asset/font/afi-fonts.css" />
    <link rel="stylesheet" href="~/asset/css/vote/common.css" />
    <link rel="stylesheet" href="~/asset/css/vote-form.css" />
  </head>

  <body>
   <section class="vote-result">
      <div class="button-container">
        <h1>Vote Result</h1>
        <button class="export-button" id="csvDownloadBtn">Export PDF</button>
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

          <tbody id="tBody">
           
            
          </tbody>
        </table>
      </div>
    </section>

    <section class="voteChart-container">
      <div class="container">
        <canvas id="voteChart"></canvas>
      </div>
    </section>

    <!-- ************************************************ -->
    <!--                   SCRIPT  s                 -->
    <!-- ************************************************ -->
     <!-- <script src="./js/chart.js"></script> -->
    <script type="text/javascript" src="/asset/js/chart.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/jquery.min.js"></script>
    <script type="text/javascript" src="/asset/js/jsLibrary/FileSaver.min.js"></script>

    <script>
      const ctx = document.getElementById("voteChart");
      const tBody = document.getElementById("tBody");

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
              // x: {
              //   ticks: {
              //     font: {
              //       size: 18,
              //       weight: "bold",
              //       family: "'Roboto', sans-serif",
              //     },
              //   },
              // },
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
        const url = `/api/sitecore/AFIReport/GetVoteCountReportForResult?voatingPeriodId=2`;

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
          },
          error: function (xhr, status, error) {
            alert("No Data Found!");
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
    </script>

  
  </body>
</html>
