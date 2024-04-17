<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Broken Link</title>
    <style>
      #error-alert-container {
        position: fixed;
        z-index: 9999;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        background: #000;
        color: #fff;
        padding: 30px;
        border-radius: 10px;
        max-width: 800px;
        width: 90%;
        box-shadow: 1px 1px 10px 5px rgba(0, 0, 0, 0.2);
      }

      #error-alert-container #close-btn {
        position: absolute;
        right: 10px;
        top: 5px;
        line-height: 0;
        width: 20px;
        height: 20px;
        border: 1px solid #fff;
        display: grid;
        place-content: center;
        border-radius: 50%;
        cursor: pointer;
        font-size: 12px;
      }

      #error-table {
        width: 100%;
        border-collapse: collapse;
      }

      #error-table th,
      #error-table td {
        padding: 5px;
        max-width: 350px;
        overflow-wrap: break-word;
        border: 1px solid #ccc;
      }
    </style>
  </head>

  <body>
    <!-- ******************* Error Table Popup ******************* -->
    <section id="error-alert-container" style="display: none">
      <span id="close-btn">X</span>

      <table id="error-table" style="width: 100%">
        <thead>
          <tr>
            <th>Text</th>
            <th>URL</th>
          </tr>
        </thead>

        <tbody></tbody>
      </table>
    </section>

    <script
      type="text/javascript"
      src="/asset/js/jsLibrary/jquery.min.js"
    ></script>
    <script>
      $(document).ready(function () {
        var htmlEl = `<section>
                    <p>Internal Link : <a href="~/link.aspx?_id=B141D8787D304C1280A20D9A08A6D9A1&amp;_z=z">This is sample</a></p>
                    <p>Internal Link : <a href="~/link.aspx?_id=A7FF0040F9094A65AF81FD0EB61DFAF7&amp;_z=z">This is sample</a></p>
                    <p>Internal Link : <a href="~/link.aspx?_id=8E8C6F8D049F4FTR45AE67444DB77088&amp;_z=z">This is sample</a></p>
                    <p>Internal Link : <a href="~/link.aspx?_id=CE60CFED0B0849B1BF9E6245806FD006&amp;_z=z">This is sample</a></p>
                    <p>External Link : <a href="http://www.google.com">Google.com</a></p>
                    <p>External Link : <a href="https://www.google.com">Google.com</a></p>
                    <p>External Link : <a href="https://www.adecco.co.uk/">Adecco UK</a></p>
                    <p>&nbsp;</p>
                    <p>Broken Internal Link : <a href="~/link.aspx?_id=0E8B09C46D754C8AA2E6FB817C0F1E5BAA&amp;_z=z">This is Broken sample Internal Link</a></p>
                    <p>Broken External Link : <a href="http://www.google.comuuuu">This is broken google.com</a></p>
                </section>`;

        // Extract URLs from the HTML content
        var urls = [];
        $(htmlEl)
          .find("a")
          .each(function () {
            urls.push($(this).attr("href"));
          });

        // Send only the URLs as a comma-separated string
        var urlList = urls.join(",");

        $.ajax({
          url: "/api/sitecore/Forms/ProcessJsonArray",
          type: "POST",
          data: { urlList: urlList }, // Send URLs as a comma-separated string
          success: function (response) {
            if (Array.isArray(response)) {
              response.forEach((item) => {
                // if validation fails then add to table
                if (item[3] === "False") {
                  addDataToErrorTable(item);
                }
              });
            }
          },
          error: function (xhr, status, error) {
            // Handle errors here
            console.error(xhr.responseText);
          },
        });
      });
    </script>
  </body>
</html>
